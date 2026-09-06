using System;
using Xunit;
using SSITechnicalAssessment.Shared.Services;
using System.Collections.Generic;
using SSITechnicalAssessment.Shared.Models.ClaimModel;
using System.IO;

namespace SSITechnicalAssessment.Tests
{
    public class ParseSegmentsTests
    {
        [Fact]
        public void ParseSegments_SingleClaimSegment_ShouldReturnSingleClaim()
        {
            string CLMSegment = "CLM*XYZ123000456*25***01:B:1*N*C*Y*Y~";

            // Ensure there is only one claim first
            List<Claim> claims = EDI837Parser.ParseSegments(CLMSegment);
            Assert.Single(claims);

            // Validate claim values
            Claim claim = claims[0];
            Assert.Equal("XYZ123000456", claim.PatientAccountNum);
            Assert.Equal(25m, claim.ClaimChargeAmt);
            Assert.Equal("01", claim.Facility.FacilityTypeCode);
            Assert.Equal('B', claim.Facility.FacilityCodeQual);
            Assert.Equal("1", claim.Facility.ClaimFreqCode);
            Assert.Equal('N', claim.SignatureIndicator);
            Assert.Equal('C', claim.ParticipationCode);
            Assert.Equal('Y', claim.BenefitsAsmntCertIndicator);
            Assert.Equal('Y', claim.ReleaseInfoIndicator);
        }

        [Fact]
        public void ParseSegments_NoSegments_ShouldReturnNoClaims()
        {
            string EmptyCLMSegment = "";

            List<Claim> claims = EDI837Parser.ParseSegments(EmptyCLMSegment);
            Assert.Empty(claims);
        }

        [Fact]
        public void ParseSegments_MultipleClaimSegments_ShouldReturnAllClaims()
        {
            string CLMSegments = "CLM*XYZ123000456*25***01:B:1*N*C*Y*Y~CLM*XYZ123000567*10***01:B:1*N*C*Y*Y~";

            List<Claim> claims = EDI837Parser.ParseSegments(CLMSegments);
            Assert.Equal(2, claims.Count);

            Claim claim1 = claims[0];
            Claim claim2 = claims[1];
            Assert.Equal("XYZ123000456", claim1.PatientAccountNum);
            Assert.Equal("XYZ123000567", claim2.PatientAccountNum);
        }

        [Fact]
        public void ParseSegments_MultipleClaimSegments_ShouldReturnCombinedChargeAmount()
        {
            string CLMSegments = "CLM*XYZ123000456*25***01:B:1*N*C*Y*Y~CLM*XYZ123000567*10***01:B:1*N*C*Y*Y~";

            List<Claim> claims = EDI837Parser.ParseSegments(CLMSegments);
            Claim claim1 = claims[0];
            Claim claim2 = claims[1];

            decimal total = EDI837Parser.GetTotalChargeAmtAllClaims(claims);
            decimal expected = 35;
            Assert.Equal(expected, total);

        }

        [Fact]
        public void ParseSegments_MultipleSegmentIdentifiers_ShouldIgnoreNonClaimSegments()
        {
            string MultipleCLMSegmentIDs = "GS*HC*SSIGROUP*SSIGROUP*20220120*1544*1*X*005010X222A1~CLM*XYZ123000456*25***01:B:1*N*C*Y*Y~";

            List<Claim> claims = EDI837Parser.ParseSegments(MultipleCLMSegmentIDs);
            Assert.Single(claims);

            Claim claim = claims[0];
            Assert.Equal("XYZ123000456", claim.PatientAccountNum);
        }

        [Fact]
        public void ParseSegments_MalformedCLMSegment_ShouldSkipAndReturnNoClaims()
        {
            string NonCLMSegment = "CLM* ACC001*NOTANUMBER * **01:B: 1 * Y * A * Y * Y~";

            List<Claim> claims = EDI837Parser.ParseSegments(NonCLMSegment);
            Assert.Empty(claims);
        }
    }
}
