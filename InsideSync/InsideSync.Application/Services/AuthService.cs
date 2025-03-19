using InsideSync.Application.DTOs;
using InsideSync.Application.Interfaces.Authentication;
using InsideSync.Application.Interfaces.Email;
using InsideSync.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsideSync.Application.Services
{
    public class AuthService
    {
        private IAuthRepository _repository;
        private IEmailSender _emailSender;
        public AuthService(IAuthRepository repository, IEmailSender emailSender)
        {
            _repository = repository;
            _emailSender = emailSender;
        }

        public async Task<string> GenerateOTPByEmailAsync(string email)
        {
            var otp = await _repository.GenerateOTPByEmailAsync(email);

            if (string.IsNullOrEmpty(otp) || otp == "401")
                return otp;

            var emailMessage = new EmailMessage
            {
                To = email,
                Body = $"Your OTP is {otp}",
                Subject = "One-time-pass code from InsideSync"
            };
            await _emailSender.SendEmail(emailMessage);

            return otp;
        }

        public async Task<string> ValidateOTPByEmailAsync(string email, string otp)
        {
            return await _repository.ValidateOTPByEmailAsync(email, otp);
        }
    }
}
