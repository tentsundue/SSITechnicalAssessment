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
            Console.WriteLine("Enter the path to the EDI 837 file:");
            string path = Console.ReadLine() ?? "";
            if (path.Length == 0)
            {
                Console.WriteLine("ERROR: File path is required. Please try again.");
                return;
            }

            Console.WriteLine($"Parsing file: {path}\n");

            EDI837Parser parser = new EDI837Parser();
            List<Claim> retrievedClaims = parser.ParseClaims(path);

            OutputWriter.Display(retrievedClaims);

        }
    }
}
