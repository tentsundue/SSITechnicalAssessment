using System;
using System.IO;
using System.Collections.Generic;
using SSITechnicalAssessment.Models.ClaimModel;

namespace SSITechnicalAssessment.EDIParser
{
    public class EDI837Parser
    {
        const char SegmentTerminator = '~';
        const char ElementSeparator = '*';
        const char CompositeElementSeparator = ':';
        readonly List<Claim> claims = new();

        public EDI837Parser() { }

        public List<Claim> Parse(string path)
        {
            if (!File.Exists(path))
            {
                Console.WriteLine("Path not valid");
                Console.ReadLine();
                return claims;
            }
            string textContent = File.ReadAllText(path);
            Console.WriteLine(textContent);
            Console.ReadLine();

            return claims;
        }
    }
}