using System;
using System.IO;
using SSITechnicalAssessment.EDIParser;

namespace SSITechnicalAssessment
{
    class Program
    {
        static void Main(string[] args)
        {
            string[] paths = {"EDI837Files", "SampleProfessional.837" };
            string full_path = Path.Combine(paths);
            Console.WriteLine(full_path);
            Console.ReadLine();
       
            EDI837Parser parser = new EDI837Parser();
            parser.Parse(full_path);
        }
    }
}
