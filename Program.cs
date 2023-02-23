using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Confluent.Kafka;
using Confluent.Kafka.DependencyInjection;
using EscortBookMessenger.Backgrounds;
using EscortBookMessenger.Services;
using EscortBookMessenger.Constants;

namespace EscortBookMessenger;

public class Program
{
    public static void Main(string[] args)
    {
        IHost host = CreateHostBuilder(args).Build();
        host.Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureServices((hostContext, services) =>
            {
                services.AddSingleton<IMessenger, Messenger>();
                services.AddHostedService<KafkaMessengerConsumer>();
                services.AddKafkaClient(new ConsumerConfig
                {
                    GroupId = KafkaConfig.GroupId,
                    BootstrapServers = KafkaConfig.Servers
                });
            });
}
