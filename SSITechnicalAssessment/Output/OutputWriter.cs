using SSITechnicalAssessment.Models.ClaimModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSITechnicalAssessment.Output
{
    class OutputWriter
    {
        const string CategoryDivider    = "==================================================================";
        const string ClaimInfoDivider   = "----------------------------------------------------------";
     
        public OutputWriter() { }

        public static void Display(List<Claim> claims, decimal totalChargeAmtAllClaims)
        {
            int claimCount = 1;
            Console.WriteLine($"{CategoryDivider}\n");
            foreach (Claim claim in claims)
            {
                Console.WriteLine($"CLAIM {claimCount++}:");
                Console.WriteLine(ClaimInfoDivider);
                Console.WriteLine($"Patient Account: {claim.PatientAccountNum}\t| Claim Charge Amount: ${claim.ClaimChargeAmt}");
                Console.WriteLine();

                Console.WriteLine($"Facility Type Code: {claim.Facility.FacilityTypeCode}");
                Console.WriteLine($"Facility Code Qualifier: {claim.Facility.FacilityCodeQual}");
                Console.WriteLine($"Claim Frequency Code: {claim.Facility.ClaimFreqCode}");
                Console.WriteLine();

                Console.WriteLine($"Provider/Supplier Signature Indicator: {claim.SignatureIndicator}");
                Console.WriteLine($"Assignment/Plan Participation Code: {claim.ParticipationCode}");
                Console.WriteLine($"Benefits Assignment Certification Indicator: {claim.BenefitsAsmntCertIndicator}");
                Console.WriteLine($"Release of Information Indicator: {claim.ReleaseInfoIndicator}");

                Console.WriteLine($"{ClaimInfoDivider}\n");
            }

            Console.WriteLine($"{CategoryDivider}\n");
            Console.WriteLine($"Total Claim Charge Amount Across All Claims: ${totalChargeAmtAllClaims}");
            Console.WriteLine($"\n{CategoryDivider}");
        }
    }
}
