using System;
using System.Collections.Generic;
using System.IO;
using SSITechnicalAssessment.Shared.Services;
using SSITechnicalAssessment.Shared.Models.ClaimModel;
using SSITechnicalAssessment.Output;

namespace SSITechnicalAssessment
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the path to the EDI 837 file:");
            string path = Console.ReadLine() ?? "";
            if (path.Length == 0)
            {
                Console.WriteLine("ERROR: File path is required. Please try again.");
                return;
            }

            Console.WriteLine($"Parsing file: {path}\n");

            List<Claim> retrievedClaims = EDI837Parser.ParseFile(path);
            decimal totalChargeAmtAllClaims = EDI837Parser.GetTotalChargeAmtAllClaims(retrievedClaims); 

            OutputWriter.Display(retrievedClaims, totalChargeAmtAllClaims);

        }
    }
}
