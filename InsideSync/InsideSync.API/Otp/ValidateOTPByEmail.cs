using InsideSync.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace InsideSync.API.Otp
{
  public class ValidateOTPByEmail
  {
    private readonly ILogger<ValidateOTPByEmail> _logger;
    private readonly OtpManager _otpManager;

    public ValidateOTPByEmail(ILogger<ValidateOTPByEmail> logger, OtpManager optManager)
    {
      _logger = logger;
      _otpManager = optManager;
    }

    [Function("ValidateOTPByEmail")]
    public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", "post")] HttpRequest req)
    {
      _logger.LogInformation("C# HTTP trigger function processed a request.");

      var headers = req.Headers;

      if (!headers.ContainsKey("Email") || !headers.ContainsKey("Otp"))
        return new UnauthorizedObjectResult(401);

      string? email = headers["Email"];
      string? otp = headers["Otp"];

      var token = await _otpManager.ValidateOTPByEmailAsync(email, otp);

      if (token == "401")
        return new UnauthorizedObjectResult(401);

      return new OkObjectResult(token);
    }
  }
}
