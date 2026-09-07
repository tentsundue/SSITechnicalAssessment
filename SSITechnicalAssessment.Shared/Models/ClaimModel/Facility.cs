using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SSITechnicalAssessment.Shared.Models.ClaimModel
{
    public class Facility
    {
        public int ClaimID { get; set; } // Used for the WebApp version
        public string FacilityTypeCode { get; }
        public char FacilityCodeQual { get; }
        public int ClaimFreqCode { get; }

        public Facility(string facilityTypeCode, char facilityCodeQual, int claimFreqCode)
        {
            FacilityTypeCode = facilityTypeCode;
            FacilityCodeQual = facilityCodeQual;
            ClaimFreqCode = claimFreqCode;
        }
    };
}
