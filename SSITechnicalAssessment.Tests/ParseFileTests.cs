using System;
using Xunit;
using SSITechnicalAssessment.EDIParser;
using System.Collections.Generic;
using System.IO;

namespace SSITechnicalAssessment.Tests
{
    public class ParseFileTests
    {
        [Fact]
        public void ParseFile_NoFileGiven_ShouldThrowFileNotFoundException()
        {
            string nonExistentPath = "EDI/Path";

            Assert.Throws<FileNotFoundException>(() => EDI837Parser.ParseFile(nonExistentPath));
        }
    }
}
