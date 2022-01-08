using Coravel;
using EscortBookMessenger.Jobs;
using EscortBookMessenger.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace EscortBookMessenger
{
    public class Program
    {
        public static void Main(string[] args)
        {
            IHost host = CreateHostBuilder(args).Build();
            host.Services.UseScheduler(scheduler =>
            {
                scheduler
                    .Schedule<MessengerJob>()
                    .EveryMinute();
            });
            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    IConfiguration configuration = hostContext.Configuration;

                    services.AddScheduler();
                    services.AddTransient<MessengerJob>();
                    services.AddTransient<IAWSSQSService, AWSSQSService>();
                    services.AddTransient<IMessenger, Messenger>();
                });
    }
}
