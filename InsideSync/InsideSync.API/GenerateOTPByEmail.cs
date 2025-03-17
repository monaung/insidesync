using InsideSync.Application.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace InsideSync.API
{
    public class GenerateOTPByEmail
    {
        private readonly ILogger<GenerateOTPByEmail> _logger;
        private readonly AuthService _authService;
        public GenerateOTPByEmail(ILogger<GenerateOTPByEmail> logger, AuthService authService)
        {
            _logger = logger;
            _authService = authService;
        }

        [Function("GenerateOTPByEmail")]
        public async Task<IActionResult> Run([HttpTrigger(AuthorizationLevel.Function, "get", Route ="otp")] HttpRequest req)
        {
            string email = req.Query["email"];
            _logger.LogInformation($"Generrate OTP By Email: {email}");

            if (string.IsNullOrEmpty(email))
            {
                return new UnauthorizedObjectResult(401);
            }

            var otp = await _authService.GenerateOTPByEmailAsync(email);

            if(otp == "401")
                return new UnauthorizedObjectResult(401);

            return new OkObjectResult(otp);
        }
    }
}
