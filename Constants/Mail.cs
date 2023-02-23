using System;

namespace EscortBookMessenger.Constants;

public class Mail
{
    public static readonly string From = Environment.GetEnvironmentVariable("MAIL_SERVER_FROM");

    public static readonly string Password = Environment.GetEnvironmentVariable("MAIL_SERVER_PASSWORD");
}
