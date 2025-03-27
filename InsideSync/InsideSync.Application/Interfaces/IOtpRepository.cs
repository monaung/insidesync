namespace InsideSync.Application.Interfaces
{
  public interface IOtpRepository
  {
    Task<string> GenerateOTPByEmailAsync(string Email);
    Task<string> ValidateOTPByEmailAsync(string Email, string OTP);
  }
}
