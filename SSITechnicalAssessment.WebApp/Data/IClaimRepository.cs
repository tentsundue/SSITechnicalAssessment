using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SSITechnicalAssessment.Shared.Models.ClaimModel;
namespace SSITechnicalAssessment.WebApp.Data
{
    public interface IClaimRepository
    {
        Task<int> PostClaimAsync(Claim claim);
        Task<IReadOnlyList<Claim>> GetAllClaimsAsync();
    }
}
