using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
<<<<<<< HEAD
using Services.Interfaces;
=======
using Services.Interface;
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Utilities;

namespace Services.Service
{
    public class MailService : IMailService
    {
        private readonly MailSettings _mailSettings;

        public MailService(IOptions<MailSettings> mailSettings)
        {
            this._mailSettings = mailSettings.Value;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
<<<<<<< HEAD
            var email = new MimeMessage();
            email.Sender = MailboxAddress.Parse(_mailSettings.SenderMail);
            email.To.Add(MailboxAddress.Parse(toEmail));
            email.Subject = subject;
            email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
            smtp.Authenticate(_mailSettings.SenderMail, _mailSettings.SenderMailPassword);
            await smtp.SendAsync(email);
            smtp.Disconnect(true);
=======
                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.SenderMail);
                email.To.Add(MailboxAddress.Parse(toEmail));
                email.Subject = subject;
                email.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = body };

                using var smtp = new SmtpClient();
                await smtp.ConnectAsync(_mailSettings.Server, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.SenderMail, _mailSettings.SenderMailPassword);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
>>>>>>> 69142915af0cedae9b642d72a42af8d86afd3ec1
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
