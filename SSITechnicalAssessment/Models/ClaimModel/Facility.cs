using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSITechnicalAssessment.Models.ClaimModel
{
    public record Facility
    (
        string FacilityTypeCode, 
        char FacilityCodeQual,
        string ClaimFreqCode
    );
}
