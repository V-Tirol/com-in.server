using MimeKit;
using MailKit.Net.Smtp;
using System.Reflection.Metadata;
using MailKit.Security;
using Org.BouncyCastle.Tls;

namespace com_in.server.Service
{
    public interface IEmailService
    {
        Task SendEmailAsync(string email, string subject, string htmlMessage);
    }

    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        public EmailService(IConfiguration configuration) => _configuration = configuration;

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            using var client = new SmtpClient();


            var SenderName = _configuration["EmailSettings:SenderName"];
            var SenderMail = _configuration["EmailSettings:SenderEmail"];
            var username = _configuration["EmailSettings:Username"];
            var password = _configuration["EmailSettings:Password"];

            //_configuration["EmailSettings:SenderName"],
            //        _configuration["EmailSettings:SenderMail"]

            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress(SenderName, SenderMail));
                message.To.Add(new MailboxAddress("", email));
                message.Subject = subject;

                var builder = new BodyBuilder { HtmlBody = htmlMessage };
                message.Body = builder.ToMessageBody();

                

                await client.ConnectAsync(
                    _configuration["EmailSettings:MailServer"],
                    int.Parse(_configuration["EmailSettings:MailPort"]),
                    SecureSocketOptions.StartTls);

                await client.AuthenticateAsync(username, password);

                await client.SendAsync(message);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                await client.DisconnectAsync(true);
            }
        }
    }
}
