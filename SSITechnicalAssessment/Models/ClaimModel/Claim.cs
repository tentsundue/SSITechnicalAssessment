using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSITechnicalAssessment.Models.ClaimModel
{
    public record Claim
    (
        string PatientAccountNum,
        decimal ClaimChargeAmt,
        Facility Facility,
        char SignatureIndicator,
        char ParticipationCode,
        char BenefitsAsmntCertIndicator,
        char ReleaseInfoIndicator,
        string? CLM03 = null,
        string? CLM04 = null
    );
}
