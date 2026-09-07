using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using SSITechnicalAssessment.Shared.Models.ClaimModel;

namespace SSITechnicalAssessment.WebApp.Data
{
    public class ClaimRepository : IClaimRepository
    {
        private readonly string _connectionString;

        public ClaimRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SSITechnicalAssessmentDB")
                ?? throw new InvalidOperationException(
                    "Connection string 'SSITechnicalAssessmentDB' not found in appsettings.json");
        }

        public async Task<int> PostClaimAsync(Claim claim)
        {
            await using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();
            await using SqlTransaction transaction = conn.BeginTransaction(); // Note: this is so that I can bind both insertions into one single unit of work.

            string insertClaimQuery = @"
                    INSERT INTO Claim
                     (PatientAccountNum, 
                      ClaimChargeAmt, 
                      SignatureIndicator, 
                      ParticipationCode,
                      BenefitsAsmntCertIndicator, 
                      ReleaseInfoIndicator, 
                      CLM03, 
                      CLM04)
                    VALUES
                     (@PatientAccountNum, 
                      @ClaimChargeAmt, 
                      @SignatureIndicator, 
                      @ParticipationCode,
                      @BenefitsAsmntCertIndicator, 
                      @ReleaseInfoIndicator, 
                      @Clm03, 
                      @Clm04);
                    SELECT SCOPE_IDENTITY() AS NewID;";
                                         
            string insertFacilityQuery = @"
                    INSERT INTO Facility
                     (ClaimID, 
                      FacilityTypeCode, 
                      FacilityCodeQual, 
                      ClaimFreqCode)
                    VALUES 
                     (@ClaimID, 
                      @FacilityTypeCode, 
                      @FacilityCodeQual, 
                      @ClaimFreqCode);";
            try
            {
                int newClaimID;

                await using (SqlCommand cmd = new SqlCommand(insertClaimQuery,
                    conn, transaction))
                {
                    cmd.Parameters.AddWithValue("@PatientAccountNum", claim.PatientAccountNum);
                    cmd.Parameters.AddWithValue("@ClaimChargeAmt", claim.ClaimChargeAmt);
                    cmd.Parameters.AddWithValue("@SignatureIndicator", claim.SignatureIndicator.ToString());
                    cmd.Parameters.AddWithValue("@ParticipationCode", claim.ParticipationCode.ToString());
                    cmd.Parameters.AddWithValue("@BenefitsAsmntCertIndicator", claim.BenefitsAsmntCertIndicator.ToString());
                    cmd.Parameters.AddWithValue("@ReleaseInfoIndicator", claim.ReleaseInfoIndicator.ToString());
                    cmd.Parameters.AddWithValue("@Clm03", (object?)claim.CLM03 ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@Clm04", (object?)claim.CLM04 ?? DBNull.Value);

                    var res = await cmd.ExecuteScalarAsync();
                    newClaimID = Convert.ToInt32(res);
                }

                await using (SqlCommand cmd2 = new SqlCommand(insertFacilityQuery,
                    conn, transaction))
                {
                    cmd2.Parameters.AddWithValue("@ClaimID", newClaimID);
                    cmd2.Parameters.AddWithValue("@FacilityTypeCode", claim.Facility.FacilityTypeCode);
                    cmd2.Parameters.AddWithValue("@FacilityCodeQual", claim.Facility.FacilityCodeQual);
                    cmd2.Parameters.AddWithValue("@ClaimFreqCode", claim.Facility.ClaimFreqCode);

                    await cmd2.ExecuteNonQueryAsync();
                }

                await transaction.CommitAsync();
                return newClaimID;
            }
            catch (Exception e)
            {
                Console.WriteLine($"ERROR: {e}");
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IReadOnlyList<Claim>> GetAllClaimsAsync()
        {
            List<Claim> allClaims = new List<Claim>();

            await using SqlConnection conn = new SqlConnection(_connectionString);
            await conn.OpenAsync();

            string getAllClaimsQuery = @"
                    SELECT 
                     c.ClaimID, 
                     c.PatientAccountNum, 
                     c.ClaimChargeAmt, 
                     c.SignatureIndicator, 
                     c.ParticipationCode,
                     c.BenefitsAsmntCertIndicator, 
                     c.ReleaseInfoIndicator, 
                     f.FacilityTypeCode, 
                     f.FacilityCodeQual, 
                     f.ClaimFreqCode,
                     c.CLM03, 
                     c.CLM04
                    FROM Claim AS c
                    JOIN Facility AS f
                     ON c.ClaimID = f.ClaimID
                    ORDER BY c.ClaimID ASC;";

            await using SqlCommand cmd = new SqlCommand(getAllClaimsQuery, conn);

            await using var reader = await cmd.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                string PatientAccountNum = reader.GetString(1);
                decimal ClaimChargeAmt = reader.GetDecimal(2);
                char SignatureIndicator = reader.GetString(3)[0];
                char ParticipationCode = reader.GetString(4)[0];
                char BenefitsAsmntCertIndicator = reader.GetString(5)[0];
                char ReleaseInfoIndicator = reader.GetString(6)[0];

                string FacilityTypeCode = reader.GetString(7);
                char FacilityCodeQual = reader.GetString(8)[0];
                int ClaimFreqCode = reader.GetInt32(9);

                string? CLM03 = reader.IsDBNull(10) ? null : reader.GetString(10);
                string? CLM04 = reader.IsDBNull(11) ? null : reader.GetString(11);

                Facility Facility = new Facility(FacilityTypeCode, FacilityCodeQual, ClaimFreqCode);

                Claim claim = new Claim(
                    PatientAccountNum,
                    ClaimChargeAmt,
                    Facility,
                    SignatureIndicator,
                    ParticipationCode,
                    BenefitsAsmntCertIndicator,
                    ReleaseInfoIndicator,
                    CLM03,
                    CLM04
                );
                int claimID = reader.GetInt32(0);
                claim.ClaimID = claimID;

                allClaims.Add(claim);
            }

            return allClaims;
        }
    }
}