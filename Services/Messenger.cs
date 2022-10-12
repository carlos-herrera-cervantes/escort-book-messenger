using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EscortBookMessenger.Models;

namespace EscortBookMessenger.Services;

public class Messenger : IMessenger
{
    #region snippet_ActionMethods

    public async Task SendEmailAsync(RequestorsMessage requestorsMessage)
    {
        var (to, subject, body) = requestorsMessage;
        var from = Environment.GetEnvironmentVariable("MAIL_SERVER_FROM");

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
                Environment.GetEnvironmentVariable("MAIL_SERVER_PASSWORD")
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
