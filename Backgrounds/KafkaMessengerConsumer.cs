using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using EscortBookMessenger.Models;
using EscortBookMessenger.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace EscortBookMessenger.Backgrounds
{
    public class KafkaMessengerConsumer : BackgroundService
    {
        #region snippet_Properties

        private readonly IConfiguration _configuration;

        private readonly ILogger _logger;

        private readonly IMessenger _messenger;

        #endregion

        #region snippet_Constructors

        public KafkaMessengerConsumer
        (
            IConfiguration configuration,
            ILogger<KafkaMessengerConsumer> logger,
            IMessenger messenger
        )
        {
            _configuration = configuration;
            _logger = logger;
            _messenger = messenger;
        }

        #endregion

        #region snippet_ActionMethods

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _configuration["Kafka:GroupId"],
                BootstrapServers = _configuration["Kafka:Servers"],
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var builder = new ConsumerBuilder<Ignore, string>(config).Build();
            builder.Subscribe(_configuration["Kafka:Topics:SendEmail"]);

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
}
