using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSITechnicalAssessment.Models.ClaimModel
{
    public record Claim(
        string PatientAccountNum,
        decimal ClaimChargeAmt,
        Facility Facility,
        string SignatureIndicator,
        string ParticipationCode,
        string BenefitsAsmntCertIndicator,
        string ReleaseInfoIndicator
    );
}
