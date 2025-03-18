using InsideSync.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsideSync.Application.Interfaces
{
    public interface IAuthRepository
    {
        Task<string> GenerateOTPByEmailAsync(string Email);
        Task<string> ValidateOTPByEmailAsync(string Email, string OTP);
    }
}
