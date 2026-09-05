using System;
using System.Collections.Generic;
using System.IO;
using SSITechnicalAssessment.EDIParser;
using SSITechnicalAssessment.Models.ClaimModel;
using SSITechnicalAssessment.Output;

namespace SSITechnicalAssessment
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] paths = {"EDI837Files", "SampleProfessional.837" };
            string full_path = Path.Combine(paths);
  
            EDI837Parser parser = new EDI837Parser();
            List<Claim> retrievedClaims = parser.ParseClaims(full_path);

            OutputWriter outputwriter = new OutputWriter();
            OutputWriter.Display(retrievedClaims);

        }
    }
}
