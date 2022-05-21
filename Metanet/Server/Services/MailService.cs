using MailKit.Net.Smtp;
using MailKit.Security;
using Metanet.Server.Database;
using Metanet.Server.Helpers;
using Metanet.Shared.DTO;
using Metanet.Shared.ResponsesDTO;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Metanet.Server.Services
{
    public class MailService : IMailService
    {

        private readonly MailSettings _mailSettings;
        ApplicationDbContext dbContext;
        public MailService(IOptions<MailSettings> mailSettings, ApplicationDbContext _context)
        {
            _mailSettings = mailSettings.Value;
            dbContext = _context;
        }

        public async Task<ServiceResponse<bool>> SendEmailAsync(MailRequest mailRequest)
        {
            try
            {
                //template
                string FilePath = Directory.GetCurrentDirectory() + "/Template/mailer.html";
                StreamReader str = new StreamReader(FilePath);
                string MailText = str.ReadToEnd();
                str.Close();
                MailText = MailText.Replace("[Theme]", mailRequest.Subject).Replace("[Body]", mailRequest.Body);

                var email = new MimeMessage();
                email.Sender = MailboxAddress.Parse(_mailSettings.Mail);
                if (mailRequest.Emails == null || mailRequest.Emails.Count == 0)
                {
                    mailRequest.Emails = dbContext.Users.Select(u => u.Email).ToList();
                }
                foreach (var receipent in mailRequest.Emails)
                {
                    email.To.Add(MailboxAddress.Parse(receipent));
                }
                email.Subject = mailRequest.Subject;
                var builder = new BodyBuilder();
                if (mailRequest.Attachments != null)
                {
                    byte[] fileBytes;
                    foreach (var file in mailRequest.Attachments)
                    {
                        if (file.Length > 0)
                        {
                            using (var ms = new MemoryStream())
                            {
                                file.CopyTo(ms);
                                fileBytes = ms.ToArray();
                            }
                            builder.Attachments.Add(file.FileName, fileBytes, ContentType.Parse(file.ContentType));
                        }
                    }
                }

                builder.HtmlBody = mailRequest.Body;
                builder.HtmlBody = MailText;
                email.Body = builder.ToMessageBody();
                using var smtp = new SmtpClient();
                smtp.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);
                smtp.Authenticate(_mailSettings.Mail, _mailSettings.Password);
                await smtp.SendAsync(email);
                smtp.Disconnect(true);
                return new ServiceResponse<bool>
                {
                    Success = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Успешно отправлено"
                };
            }
            
            catch(Exception ex)
            {
                return new ServiceResponse<bool>
                {
                    Success = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = $"Что-то пошло не так {ex}"
                };
            }
        }
    }
}
