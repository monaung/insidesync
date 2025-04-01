using InsideSync.Application.Interfaces;
using InsideSync.Application.Models.Email;
using InsideSync.Domain.Entities;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsideSync.Application.Services
{
  public class OtpManager
  {
    private readonly IEmailService _emailService;
    private readonly IOtpRepository _otpRepository;
    private readonly ILogger<OtpManager> _logger;
    public OtpManager(IEmailService emailService, IOtpRepository otpRepository, Logger<OtpManager> logger)
    {
      _emailService = emailService;
      _otpRepository = otpRepository;
      _logger = logger;
    }

    public async Task<Otp> GenerateOTPByEmailAsync(string email)
    {
      string otpCode;
      try
      {
        otpCode = await _otpRepository.GenerateOTPByEmailAsync(email);

        if (string.IsNullOrEmpty(otpCode))
          throw new Exception($"OTP Code is empty for {email}");
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, "Error generating OTP from DB.");
        throw;
      }

      var otp = new Otp
      {
        Email = email,
        Code = otpCode,
      };

      var emailMessage = new EmailMessage
      {
        To = email,
        Body = $"Your OTP is {otpCode}",
        Subject = "One-time-pass code from InsideSync"
      };

      try
      {
        await _emailService.SendEmail(emailMessage);
      }
      catch (Exception ex)
      {
        _logger.LogError(ex, $"Failed to send OTP email to {email}");
        throw;
      }

      return otp;

    }
    public async Task<string> ValidateOTPByEmailAsync(string email, string otp)
    {
      return await _otpRepository.ValidateOTPByEmailAsync(email, otp);
    }

  }
}
