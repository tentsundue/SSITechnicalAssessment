using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSITechnicalAssessment.Shared.Models.ClaimModel
{
    public class Claim
    {
        public int ClaimID { get; set; } // Used for the WebApp version
        public string PatientAccountNum { get; }
        public decimal ClaimChargeAmt { get; }
        public Facility Facility { get; }
        public char SignatureIndicator { get; }
        public char ParticipationCode { get; }
        public char BenefitsAsmntCertIndicator { get; }
        public char ReleaseInfoIndicator { get; }
        public string? CLM03 { get; }
        public string? CLM04 { get; }

        public Claim(string patientAccountNum, decimal claimChargeAmt, Facility facility,
            char signatureIndicator, char participationCode, char benefitsAsmntCertIndicator,
            char releaseInfoIndicator, string? clm03, string? clm04)
        {
            PatientAccountNum = patientAccountNum;
            ClaimChargeAmt = claimChargeAmt;
            Facility = facility;
            SignatureIndicator = signatureIndicator;
            ParticipationCode = participationCode;
            BenefitsAsmntCertIndicator = benefitsAsmntCertIndicator;
            ReleaseInfoIndicator = releaseInfoIndicator;
            CLM03 = clm03;
            CLM04 = clm04;
        }
    }
}
