using System;

namespace EscortBookMessenger.Constants;

public static class KafkaTopic
{
    public const string SendEmail = "send-email";
}

public static class KafkaConfig
{
    public static readonly string Servers = Environment.GetEnvironmentVariable("KAFKA_SERVERS");

    public static readonly string GroupId = Environment.GetEnvironmentVariable("KAFKA_GROUP_ID");
}
