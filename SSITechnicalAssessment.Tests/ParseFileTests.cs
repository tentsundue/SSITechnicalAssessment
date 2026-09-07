using System;
using Xunit;
using SSITechnicalAssessment.Shared.Services;
using System.Collections.Generic;
using System.IO;
using SSITechnicalAssessment.Shared.Types;

namespace SSITechnicalAssessment.Tests
{
    public class ParseFileTests
    {
        [Fact]
        public void ParseFile_NoFileGiven_ShouldThrowFileNotFoundException()
        {
            string nonExistentPath = "EDI/Path";

            Assert.Throws<FileNotFoundException>(() => EDI837Parser.VerifyFile(nonExistentPath));
        }
        
        [Fact]
        public void ParseFile_Not837File_ShouldThrowNot837Exception()
        {
            string not837FilePath = "TestFiles/not837.txt";

            Assert.Throws<Not837Exception>(() => EDI837Parser.VerifyFile(not837FilePath));
        }

        [Fact]
        public void ParseFile_EmptyPathGiven_ShouldThrowEmptyPathException()
        {
            string emptyPath = "";

            Assert.Throws<EmptyPathException>(() => EDI837Parser.VerifyFile(emptyPath));
        }

    }
}
