using KedaWithRabbitMQSample.Commands;
using KedaWithRabbitMQSample.Common;
using KedaWithRabbitMQSample.Common.Consumer;
using MediatR;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KedaWithRabbitMQSample.Consumer
{
    internal class OrderConsumer : ConsumerBase, IHostedService
    {
        public OrderConsumer(
            IMediator mediator,
            ConnectionFactory connectionFactory,
            RabbitMqConfiguration<IRabbitMqItem> rabbitMqConfiguration,
            ILogger logger
            ) : base(mediator, connectionFactory, rabbitMqConfiguration, logger)
        {
            try
            {
                var consumer = new EventingBasicConsumer(Channel);
                consumer.Received += OnEventReceived<OrderCommand>;
                Channel.BasicConsume(queue: rabbitMqConfiguration.QueueName, autoAck: false, consumer: consumer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while consuming message");
            }
        }

        public virtual Task StartAsync(CancellationToken cancellationToken) => Task.CompletedTask;

        public virtual Task StopAsync(CancellationToken cancellationToken)
        {
            Dispose();
            return Task.CompletedTask;
        }
    }
}