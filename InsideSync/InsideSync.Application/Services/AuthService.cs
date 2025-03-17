using InsideSync.Application.DTOs;
using InsideSync.Application.Interfaces;
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
        public AuthService(IAuthRepository repository)
        {
            _repository = repository;
        }

        public async Task<string> GenerateOTPByEmailAsync(string email)
        {
            return await _repository.GenerateOTPByEmailAsync(email);
        }
    }
}
