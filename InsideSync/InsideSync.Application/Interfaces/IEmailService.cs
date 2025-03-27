using InsideSync.Application.Models.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsideSync.Application.Interfaces
{
  public interface IEmailService
  {
    Task<bool> SendEmail(EmailMessage email);
  }
}
