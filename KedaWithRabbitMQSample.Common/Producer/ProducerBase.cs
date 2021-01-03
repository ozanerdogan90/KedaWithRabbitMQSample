using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System;
using System.Text;

namespace KedaWithRabbitMQSample.Common.Producer
{
    public abstract class ProducerBase<T> : RabbitMqClientBase<IRabbitMqItem>, IRabbitMqProducer<IRabbitMqItem>
    {
        protected ProducerBase(
            ConnectionFactory connectionFactory,
            RabbitMqConfiguration<IRabbitMqItem> rabbitMqConfiguration,
            ILogger logger
            ) : base(connectionFactory, rabbitMqConfiguration, logger)
        {
        }

        public virtual void Publish(IRabbitMqItem @event)
        {
            try
            {
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(@event));
                var properties = Channel.CreateBasicProperties();
                properties.AppId = nameof(ProducerBase<T>);
                properties.ContentType = _configuration.ContentType;
                properties.DeliveryMode = @event.DeliveryMode;
                properties.Timestamp = new AmqpTimestamp(DateTimeOffset.UtcNow.ToUnixTimeSeconds());
                Channel.BasicPublish(exchange: _configuration.QueueExchange, routingKey: _configuration.RoutingKey, body: body, basicProperties: properties);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while publishing");
            }
        }
    }
}