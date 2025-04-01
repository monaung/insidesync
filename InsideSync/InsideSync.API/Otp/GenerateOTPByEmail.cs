using Grpc.Core;
using InsideSync.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;

namespace InsideSync.API.Otp
{
  public class GenerateOTPByEmail
  {
    private readonly ILogger<GenerateOTPByEmail> _logger;
    private readonly OtpManager _otpManager;
    public GenerateOTPByEmail(ILogger<GenerateOTPByEmail> logger, OtpManager optManager)
    {
      _logger = logger;
      _otpManager = optManager;
    }

    [Function("GenerateOTPByEmail")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route = "otp")] HttpRequest req)
    {
      string email = req.Headers["email"];// req.Query["email"];


      if (string.IsNullOrEmpty(email))
      {
        return new UnauthorizedObjectResult("Email is required.");
      }

      _logger.LogInformation($"Hello, Generating OTP for: {email} is on the way!");

      try
      {
        var otp = await _otpManager.GenerateOTPByEmailAsync(email);
        return new OkObjectResult(otp);

      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Failed to generate or send OTP.");
        return new UnauthorizedObjectResult(ex.Message);
      }
    }
  }
}
