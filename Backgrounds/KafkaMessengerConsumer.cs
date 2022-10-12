using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using EscortBookMessenger.Constants;
using EscortBookMessenger.Models;
using EscortBookMessenger.Services;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EscortBookMessenger.Backgrounds;

public class KafkaMessengerConsumer : BackgroundService
{
    #region snippet_Properties

    private readonly ILogger _logger;

    private readonly IMessenger _messenger;

    #endregion

    #region snippet_Constructors

    public KafkaMessengerConsumer
    (
        ILogger<KafkaMessengerConsumer> logger,
        IMessenger messenger
    )
    {
        _logger = logger;
        _messenger = messenger;
    }

    #endregion

    #region snippet_ActionMethods

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var config = new ConsumerConfig
        {
            GroupId = Environment.GetEnvironmentVariable("KAFKA_GROUP_ID"),
            BootstrapServers = Environment.GetEnvironmentVariable("KAFKA_SERVERS"),
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var builder = new ConsumerBuilder<Ignore, string>(config).Build();
        builder.Subscribe(KafkaTopic.SendEmail);

        var cancelToken = new CancellationTokenSource();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumer = builder.Consume(cancelToken.Token);
                var requestorsMessage = JsonConvert
                    .DeserializeObject<RequestorsMessage>(consumer.Message.Value);

                await _messenger.SendEmailAsync(requestorsMessage);
            }
            catch (Exception e)
            {
                _logger.LogError("AN ERROR HAS OCCURRED SENDING AN EMAIL");
                _logger.LogError(e.Message);
                builder.Close();
            }
        }
    }

    #endregion
}
