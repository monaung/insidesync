using InsideSync.Application.Interfaces;
using InsideSync.Application.Models.Email;
using InsideSync.Domain.Entities;
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

    public OtpManager(IEmailService emailService, IOtpRepository otpRepository)
    {
      _emailService = emailService;
      _otpRepository = otpRepository;
    }

    public async Task<Otp> GenerateOTPByEmailAsync(string email)
    {
      var otpCode = await _otpRepository.GenerateOTPByEmailAsync(email);
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


      await _emailService.SendEmail(emailMessage);

      return otp;

    }
    public async Task<string> ValidateOTPByEmailAsync(string email, string otp)
    {
      return await _otpRepository.ValidateOTPByEmailAsync(email, otp);
    }

  }
}
