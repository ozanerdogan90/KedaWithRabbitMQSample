using KedaWithRabbitMQSample.Commands;
using KedaWithRabbitMQSample.Common;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace KedaWithRabbitMQSample.Consumer
{
    public class Program
    {
        public static async Task Main()
        {
            IHost host = new HostBuilder()
              .ConfigureHostConfiguration(ConfigureHostConfiguration)
              .ConfigureServices(ConfigureServices)
              .ConfigureLogging((ILoggingBuilder builder) =>
              {
                  builder.ClearProviders();
                  builder.AddConsole();
              })
              .UseConsoleLifetime()
              .Build();

            await host.RunAsync();
        }

        private static void ConfigureHostConfiguration(IConfigurationBuilder configBuilder)
        {
            configBuilder
              .AddJsonFile("appsettings.json", false)
              .AddEnvironmentVariables()
              .AddUserSecrets<Program>();
        }

        private static void ConfigureServices(HostBuilderContext hostContext, IServiceCollection services)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly())
                    .AddTransient<IRequestHandler<OrderCommand, Unit>, OrderCommandHandler>()
                    .AddHostedService<OrderConsumer>();

            var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
            services.AddScoped(_ => loggerFactory.CreateLogger(nameof(Program)));

            RegisterRabbitMq(services, hostContext.Configuration);
        }

        private static void RegisterRabbitMq(IServiceCollection services, IConfiguration configuration)
        {
            var config = new RabbitMqCredential();
            configuration.GetSection("RabbitMq").Bind(config);

            var connectionFactory = new ConnectionFactory()
            {
                HostName = config.HostName,
                UserName = config.UserName,
                Password = config.Password,
                Port = config.Port
            };

            var orderConfiguration = new RabbitMqConfiguration<IRabbitMqItem>()
            {
                QueueName = "service.order.purchase",
                QueueExchange = "service.order",
                RoutingKey = "purchase"
            };
            services.AddSingleton(_ => connectionFactory);
            services.AddSingleton(_ => orderConfiguration);
        }
    }
}