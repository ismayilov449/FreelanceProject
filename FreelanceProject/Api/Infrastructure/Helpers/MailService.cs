using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Api.Infrastructure.Helpers
{
    public interface IMailService
    {
        Task SendMailAsync(string username, string toEmail, string body);
    }

    public class MailService : IMailService
    {
        private IConfiguration _configuration;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendMailAsync(string username, string toEmail, string body)
        {
            try
            {
                string email = _configuration.GetSection("Email:projectEmail").Value;
                string passwrod = _configuration.GetSection("Email:projectPassword").Value;
                string smtp_server = _configuration.GetSection("Email:smtp_server").Value;
                int port = Int32.Parse(_configuration.GetSection("Email:port").Value);

                MimeMessage message = new MimeMessage();

                MailboxAddress from = new MailboxAddress("Freelance Portal",
                email);
                message.From.Add(from);

                MailboxAddress to = new MailboxAddress(username,
                toEmail);
                message.To.Add(to);

                message.Subject = "New job announcment";
                message.Body = new TextPart { Text = body };

                SmtpClient client = new SmtpClient();
                client.Connect(smtp_server, port, true);
                client.Authenticate(email, passwrod);


                await client.SendAsync(message);
                await client.DisconnectAsync(true);
                client.Dispose();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
