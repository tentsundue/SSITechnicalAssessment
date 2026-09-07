using System;
using System.IO;
using System.Collections.Generic;
using SSITechnicalAssessment.Shared.Models.ClaimModel;
using SSITechnicalAssessment.Shared.Types;

namespace SSITechnicalAssessment.Shared.Services
{
    public class EDI837Parser
    {
        const char SegmentDelimiter = '~';
        const char ElementDelimiter = '*';
        const char CompositeElementDelimiter = ':';
        

        public EDI837Parser() { }
        
        public static List<Claim> ParseSegments(string textContent)
        {
            List<Claim> claims = new List<Claim>();

            textContent = textContent.Replace("\r", "").Replace("\n", "");
            string[] segments = textContent.Split(SegmentDelimiter, StringSplitOptions.RemoveEmptyEntries);
            foreach (string segment in segments)
            {
                string[] elements = segment.Split(ElementDelimiter);
                string segmentID = elements[0];

                bool isCLM = string.Equals(segmentID, "CLM");
                if (isCLM)
                {
                    try
                    {
                        Claim claim = BuildClaim(elements);
                        claims.Add(claim);
                    }
                    catch (Exception e)
                    {
                        Console.WriteLine($"Error reading claim segment:\n{e}");
                        continue;
                    }
                }
            }

            return claims;
        }
        private static Claim BuildClaim(string[] elements)
        {
            string patientAccountNum = elements[1];
            decimal claimChargeAmt = decimal.Parse(elements[2]);

            string clm03 = elements[3]; // unused but still kept for future cases
            string clm04 = elements[4]; // unused but still kept for future cases

            string[] facilityInfo = elements[5].Split(CompositeElementDelimiter);
            string facilityTypeCode = facilityInfo[0];
            char facilityCodeQual = facilityInfo[1][0];
            int claimFreqCode = int.Parse(facilityInfo[2]);

            char signatureIndicator = elements[6][0];
            char participationCode = elements[7][0];
            char benefitsAsmntCertIndicator = elements[8][0];
            char ReleaseInfoIndicator = elements[9][0];

            Facility facility = new Facility(facilityTypeCode, facilityCodeQual, claimFreqCode);
            Claim claim = new Claim(patientAccountNum, claimChargeAmt, facility, signatureIndicator, participationCode, benefitsAsmntCertIndicator, ReleaseInfoIndicator, clm03, clm04);

            return claim;
        }

        public static decimal GetTotalChargeAmtAllClaims(List<Claim> claims)
        {
            decimal total = 0;
            foreach (Claim claim in claims)
            {
                total += claim.ClaimChargeAmt;
            }

            return total;
        }

        public static void VerifyFile(string path)
        {
            if (path.Length == 0)
            {
                Console.WriteLine("ERROR: File path is required. Please try again.");
                throw new EmptyPathException("No path provided.");
            }

            if (!File.Exists(path))
            {
                Console.WriteLine("ERROR: File path does not exist! Please try again.");
                throw new FileNotFoundException($"EDI file not found: {path}");
            }

            string file_ext = Path.GetExtension(path);
            if (file_ext != ".837")
            {
                Console.WriteLine("ERROR: File is not .837. Please try again.");
                throw new Not837Exception("EDI File is not .837");
            }
        }

        public static List<Claim> ParseFile(string path)
        {
            string textContent = File.ReadAllText(path);
            return ParseSegments(textContent);
        }
    }
}