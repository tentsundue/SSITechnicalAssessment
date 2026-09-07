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
            while (true)
            {
                Console.WriteLine("\nEnter the path to the EDI 837 file (Q to exit):");
                string path = Console.ReadLine() ?? "";

                if (string.Equals(path.ToLower(), "q"))
                {
                    Environment.Exit(0);
                }

                try
                {
                    EDI837Parser.VerifyFile(path);
                }
                catch (Exception e)
                {
                    continue;
                }

                Console.WriteLine($"Parsing file: {path}\n");

                List<Claim> retrievedClaims = EDI837Parser.ParseFile(path);
                decimal totalChargeAmtAllClaims = EDI837Parser.GetTotalChargeAmtAllClaims(retrievedClaims);

                OutputWriter.Display(retrievedClaims, totalChargeAmtAllClaims);
            }
        }
    }
}
