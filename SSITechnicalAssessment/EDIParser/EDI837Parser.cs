using System;
using System.IO;
using System.Collections.Generic;
using SSITechnicalAssessment.Models.ClaimModel;

namespace SSITechnicalAssessment.EDIParser
{
    public class EDI837Parser
    {
        const char SegmentDelimiter = '~';
        const char ElementDelimiter = '*';
        const char CompositeElementDelimiter = ':';

        Dictionary<string, List<string>> segmentGroups = new Dictionary<string, List<string>>();
        
        public EDI837Parser() { }

        private Claim _BuildClaim(string[] elements)
        {
            string patientAccountNum = elements[1];
            decimal claimChargeAmt = decimal.Parse(elements[2]);

            string clm03 = elements[3]; // unused but still kept for future cases
            string clm04 = elements[4]; // unused but still kept for future cases

            string[] facilityInfo = elements[5].Split(CompositeElementDelimiter);
            string facilityTypeCode = facilityInfo[0];
            char facilityCodeQual = facilityInfo[1][0];
            string claimFreqCode = facilityInfo[2];

            char signatureIndicator = elements[6][0];
            char participationCode = elements[7][0];
            char benefitsAsmntCertIndicator = elements[8][0];
            char ReleaseInfoIndicator = elements[9][0];

            Facility facility = new Facility(facilityTypeCode, facilityCodeQual, claimFreqCode);
            Claim claim = new Claim(patientAccountNum, claimChargeAmt, facility, signatureIndicator, participationCode, benefitsAsmntCertIndicator, ReleaseInfoIndicator);

            return claim;
        }

        public List<Claim> ParseClaims(string path)
        {
            List<Claim> claims = new();

            if (!File.Exists(path))
            {
                throw new FileNotFoundException($"EDI file not found: {path}", path);
            }

            string textContent = File.ReadAllText(path);
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
                        Claim claim = _BuildClaim(elements);
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
    }
}