using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Confluent.Kafka;
using EscortBookMessenger.Constants;
using EscortBookMessenger.Models;
using EscortBookMessenger.Services;
using Microsoft.Extensions.DependencyInjection;

namespace EscortBookMessenger.Backgrounds;

public class KafkaMessengerConsumer : BackgroundService
{
    #region snippet_Properties

    private readonly ILogger _logger;

    private readonly IMessenger _messenger;

    private readonly IConsumer<Ignore, string> _consumer;

    #endregion

    #region snippet_Constructors

    public KafkaMessengerConsumer
    (
        ILogger<KafkaMessengerConsumer> logger,
        IMessenger messenger,
        IServiceScopeFactory factory
    )
    {
        _logger = logger;
        _messenger = messenger;
        _consumer = factory.CreateScope().ServiceProvider.GetRequiredService<IConsumer<Ignore, string>>();
    }

    #endregion

    #region snippet_ActionMethods

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _consumer.Subscribe(KafkaTopic.SendEmail);

        var cancelToken = new CancellationTokenSource();

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                var consumer = _consumer.Consume(cancelToken.Token);
                var requestorsMessage = JsonConvert
                    .DeserializeObject<RequestorsMessage>(consumer.Message.Value);

                await _messenger.SendEmailAsync(requestorsMessage);
            }
            catch (Exception e)
            {
                _logger.LogError("AN ERROR HAS OCCURRED SENDING AN EMAIL");
                _logger.LogError(e.Message);
                _consumer.Close();
            }
        }
    }

    #endregion
}
