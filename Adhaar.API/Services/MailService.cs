using Adhaar.API.Helper;
using Adhaar.API.Models.Domain;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Adhaar.API.Services
{
    public class MailService : IMailService
    {
        private readonly EmailSettings _mailSettings;
        public MailService(IOptions<EmailSettings> mailSettingsOptions)
        {
            _mailSettings = mailSettingsOptions.Value;
        }

       /* public bool SendMail(MailRequest mailData)
        {
            try
            {
                using (MimeMessage emailMessage = new MimeMessage())
                {
                    MailboxAddress emailFrom = new MailboxAddress(_mailSettings.SenderName, _mailSettings.SenderEmail);
                    emailMessage.From.Add(emailFrom);
                    MailboxAddress emailTo = new MailboxAddress(mailData.EmailToName, mailData.EmailToId);
                    emailMessage.To.Add(emailTo);

                    emailMessage.Cc.Add(new MailboxAddress("Cc Receiver", "cc@example.com"));
                    emailMessage.Bcc.Add(new MailboxAddress("Bcc Receiver", "bcc@example.com"));

                    emailMessage.Subject = mailData.EmailSubject;

                    BodyBuilder emailBodyBuilder = new BodyBuilder();
                    emailBodyBuilder.TextBody = mailData.EmailBody;

                    emailMessage.Body = emailBodyBuilder.ToMessageBody();
                    //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
                    using (SmtpClient mailClient = new SmtpClient())
                    {
                        mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
                        mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
                        mailClient.Send(emailMessage);
                        mailClient.Disconnect(true);
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }*/

       

        public async Task<bool> SendMailAsync(MailRequest mailData)
        {
            try
            {
                

                    var email = new MimeMessage();
                    email.Sender = MailboxAddress.Parse(_mailSettings.Email);
                    email.To.Add(MailboxAddress.Parse(mailData.ToEmail));
                    email.Subject = mailData.Subject;

                    var builder = new BodyBuilder();
                    builder.HtmlBody=mailData.Body;
                    email.Body=builder.ToMessageBody();

                    using var smtp=new SmtpClient();
                    smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                    smtp.Authenticate(_mailSettings.Email, _mailSettings.Password);

                    await smtp.SendAsync(email);
                    smtp.Disconnect(true);
                

                return true;
            }
            catch (Exception ex)
            {
                // Exception Details
                return false;
            }
        }

       
    }
}
