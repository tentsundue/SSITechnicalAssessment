using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using SSITechnicalAssessment.Shared.Services;
using SSITechnicalAssessment.Shared.Models.ClaimModel;
using SSITechnicalAssessment.WebApp.Data;

namespace SSITechnicalAssessment.WebApp.Pages
{
    public class IndexModel : PageModel
    {
        private readonly IClaimRepository _ClaimRepository;
        private readonly ILogger<IndexModel> _logger;

        public string? Message { get; set; }

        public IndexModel(IClaimRepository claimRepository, ILogger<IndexModel> logger)
        {
            _ClaimRepository = claimRepository;
            _logger = logger;
        }

        public IReadOnlyList<Claim> AllClaims { get; set; } = new List<Claim>();
        public async Task OnGetAsync()
        {
            Message = TempData["Message"] as string;
            AllClaims = await _ClaimRepository.GetAllClaimsAsync();
        }

        [BindProperty]
        public IFormFile? UploadedEDIFile { get; set; }

        public async Task<IActionResult> OnPostAsync()
        {
            if (UploadedEDIFile == null || UploadedEDIFile.Length == 0)
            {
                TempData["Message"] = "Please select a file.";
                return RedirectToPage();
            }

            // Read file contents and parse directly
            try
            {
                string fileContent;
                using (var reader = new StreamReader(UploadedEDIFile.OpenReadStream()))
                {
                    fileContent = await reader.ReadToEndAsync();
                }

                List<Claim> claims = EDI837Parser.ParseSegments(fileContent);
                int totalClaims = claims.Count;
                int claimsAdded = 0;
                foreach (Claim claim in claims)
                {
                    int newClaimID = await _ClaimRepository.PostClaimAsync(claim);
                    claimsAdded++;
                }

                TempData["Message"] = $"Successfully added {claimsAdded} claims | Skipped {claimsAdded}";
                AllClaims = await _ClaimRepository.GetAllClaimsAsync();

            }
            catch (Exception e)
            {
                TempData["Message"] = $"Failed to parse claims: {e.Message}";
                _logger.LogError(e, "Failed to parse claims");
                AllClaims = await _ClaimRepository.GetAllClaimsAsync();
            }

            return RedirectToPage();
        }
    }
}
