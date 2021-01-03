using KedaWithRabbitMQSample.Commands;
using KedaWithRabbitMQSample.Common;
using KedaWithRabbitMQSample.Common.Producer;
using Microsoft.Extensions.Logging;
using RabbitMQ.Client;

namespace KedaWithRabbitMQSample.Api.RabbitMq
{
    public class OrderProducer : ProducerBase<OrderCommand>
    {
        public OrderProducer(ConnectionFactory connectionFactory, RabbitMqConfiguration<IRabbitMqItem> rabbitMqConfiguration, ILogger logger) : base(connectionFactory, rabbitMqConfiguration, logger)
        {
        }
    }
}