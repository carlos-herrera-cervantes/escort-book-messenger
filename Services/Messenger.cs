using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EscortBookMessenger.Models;
using Microsoft.Extensions.Configuration;

namespace EscortBookMessenger.Services
{
    public class Messenger : IMessenger
    {
        #region snippet_Properties

        private readonly IConfiguration _configuration;

        #endregion

        #region snippet_Constructor

        public Messenger(IConfiguration configuration) => _configuration = configuration;

        #endregion

        #region snippet_ActionMethods

        public async Task SendEmailAsync(RequestorsMessage requestorsMessage)
        {
            var (to, subject, body) = requestorsMessage;
            var from = _configuration.GetSection("EmailServerCredentials").GetSection("From").Value;

            using var smtpClient = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential
                    (
                        from,
                        _configuration.GetSection("EmailServerCredentials").GetSection("Password").Value
                    )
            };

            using var mailMessage = new MailMessage(from, to);
            mailMessage.Subject = subject;
            mailMessage.Body = body;
            mailMessage.IsBodyHtml = true;

            await smtpClient.SendMailAsync(mailMessage);
        }

        #endregion
    }
}