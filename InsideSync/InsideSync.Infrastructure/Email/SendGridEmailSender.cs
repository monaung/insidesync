using InsideSync.Application.Interfaces.Email;
using InsideSync.Application.Models.Email;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InsideSync.Infrastructure.Email
{
    public class SendGridEmailSender : IEmailSender
    {
        //public EmailSetting _emailSetting { get; }
        EmailSetting _emailSetting = new EmailSetting();
        public SendGridEmailSender(IOptions<EmailSetting> emailSetting)
        {
            //_emailSetting = emailSetting.Value;
            _emailSetting.FromName = Environment.GetEnvironmentVariable("FromName")!;
            _emailSetting.ApiKey = Environment.GetEnvironmentVariable("ApiKey")!;
            _emailSetting.FromAddress = Environment.GetEnvironmentVariable("FromAddress")!; ;
        }
        public async Task<bool> SendEmail(EmailMessage email)
        {
            var client = new SendGridClient(_emailSetting.ApiKey);
            
            var to = new EmailAddress(email.To);
            var from = new EmailAddress
            {
                Email = _emailSetting.FromAddress,
                Name = _emailSetting.FromName
            };

            var message = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, email.Body);
            var response = await client.SendEmailAsync(message);

            return response.IsSuccessStatusCode;
        }
    }
}


