using InsideSync.Application.Services;
using Microsoft.AspNetCore.Http;
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

      _logger.LogInformation($"Generating OTP for: {email}");

      if (string.IsNullOrEmpty(email))
      {
        return new UnauthorizedObjectResult("invalid email!");
      }

      var otp = await _otpManager.GenerateOTPByEmailAsync(email);

      if (string.IsNullOrEmpty(otp.Code))
      {
        return new UnauthorizedObjectResult("otp code error!");
      }

      return new OkObjectResult(otp);
    }
  }
}
