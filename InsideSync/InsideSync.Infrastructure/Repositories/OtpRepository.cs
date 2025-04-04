﻿using Azure.Identity;
using InsideSync.Application.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.Identity.Client.Platforms.Features.DesktopOs.Kerberos;
using System.Data;

namespace InsideSync.Infrastructure.Repositories
{
  public class OtpRepository : IOtpRepository
  {
    private readonly string _connectionString;
    public OtpRepository(IConfiguration configuration)
    {
      _connectionString = Environment.GetEnvironmentVariable("DefaultConnection")!;
    }
    public async Task<string> GenerateOTPByEmailAsync(string Email)
    {
      var credential = new DefaultAzureCredential();

      using var conn = new SqlConnection(_connectionString);
      conn.AccessToken = (await credential.GetTokenAsync(
      new Azure.Core.TokenRequestContext(new[] { "https://database.windows.net/" }))).Token;

      await conn.OpenAsync();

      using var cmd = new SqlCommand("GenerateOTPByEmail", conn);
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 50)).Value = Email;

      // Output Parameter
      SqlParameter outputParam = new SqlParameter("@OTP", SqlDbType.NVarChar, 10)
      {
        Direction = ParameterDirection.Output
      };
      cmd.Parameters.Add(outputParam);

      // Return Value Parameter
      SqlParameter returnParam = new SqlParameter();
      returnParam.Direction = ParameterDirection.ReturnValue;
      returnParam.SqlDbType = SqlDbType.NVarChar;
      cmd.Parameters.Add(returnParam);

      try
      {
        await cmd.ExecuteNonQueryAsync();
      }
      catch (Exception ex)
      {
        throw ex;
      }
      // Execute the stored procedure


      // Get the output parameter value
      string otpOutput = cmd.Parameters["@OTP"].Value.ToString()!;

      return otpOutput;
    }

    public async Task<string> ValidateOTPByEmailAsync(string Email, string Otp)
    {
      using var conn = new SqlConnection(_connectionString);
      await conn.OpenAsync();

      using var cmd = new SqlCommand("ValidateOTPByEmail", conn);
      cmd.CommandType = CommandType.StoredProcedure;
      cmd.Parameters.Add(new SqlParameter("@Email", SqlDbType.VarChar, 50)).Value = Email;
      cmd.Parameters.Add(new SqlParameter("@Otp", SqlDbType.NVarChar, 50)).Value = Otp;

      // Output Parameter
      SqlParameter outputParam = new SqlParameter("@Token", SqlDbType.VarChar, 50)
      {
        Direction = ParameterDirection.Output
      };
      cmd.Parameters.Add(outputParam);

      // Return Value Parameter
      SqlParameter returnParam = new SqlParameter();
      returnParam.Direction = ParameterDirection.ReturnValue;
      returnParam.SqlDbType = SqlDbType.VarChar;
      cmd.Parameters.Add(returnParam);

      try
      {
        await cmd.ExecuteNonQueryAsync();
      }
      catch (Exception ex)
      {
        throw;
      }

      // Get the output parameter value
      string otpOutput = cmd.Parameters["@Token"].Value.ToString()!;

      return otpOutput;
    }
  }
}
