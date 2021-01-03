using KedaWithRabbitMQSample.Api.RabbitMq;
using KedaWithRabbitMQSample.Commands;
using KedaWithRabbitMQSample.Common;
using KedaWithRabbitMQSample.Common.Producer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace KedaWithRabbitMQSample.Api
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers();
            services.AddTransient<IOrderService, OrderService>();
            services.AddSingleton<IRabbitMqProducer<OrderCommand>, OrderProducer>();
            services.AddSwaggerGen();
            var loggerFactory = services.BuildServiceProvider().GetRequiredService<ILoggerFactory>();
            services.AddSingleton(_ => loggerFactory.CreateLogger(nameof(Program)));
            RegisterRabbitMq(services, Configuration);
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });
            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void RegisterRabbitMq(IServiceCollection services, IConfiguration configuration)
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