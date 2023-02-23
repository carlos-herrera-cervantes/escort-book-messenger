using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using EscortBookMessenger.Models;
using EscortBookMessenger.Constants;

namespace EscortBookMessenger.Services;

public class Messenger : IMessenger
{
    #region snippet_ActionMethods

    public async Task SendEmailAsync(RequestorsMessage requestorsMessage)
    {
        var (to, subject, body) = requestorsMessage;

        using var smtpClient = new SmtpClient
        {
            Host = "smtp.gmail.com",
            Port = 587,
            EnableSsl = true,
            DeliveryMethod = SmtpDeliveryMethod.Network,
            UseDefaultCredentials = false,
            Credentials = new NetworkCredential(Mail.From, Mail.Password)
        };

        using var mailMessage = new MailMessage(Mail.From, to);
        mailMessage.Subject = subject;
        mailMessage.Body = body;
        mailMessage.IsBodyHtml = true;

        await smtpClient.SendMailAsync(mailMessage);
    }

    #endregion
}
