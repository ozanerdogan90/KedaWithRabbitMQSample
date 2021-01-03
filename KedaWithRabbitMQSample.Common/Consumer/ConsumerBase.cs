using MediatR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;

namespace KedaWithRabbitMQSample.Common.Consumer
{
    public abstract class ConsumerBase : RabbitMqClientBase<IRabbitMqItem>
    {
        private readonly IMediator _mediator;

        protected ConsumerBase(IMediator mediator, ConnectionFactory connectionFactory, RabbitMqConfiguration<IRabbitMqItem> configuration, ILogger logger) : base(connectionFactory, configuration, logger)
        {
            _mediator = mediator;
        }

        protected virtual void OnEventReceived<T>(object sender, BasicDeliverEventArgs @event)
        {
            try
            {
                var body = Encoding.UTF8.GetString(@event.Body.ToArray());
                var message = JsonConvert.DeserializeObject<T>(body);

                _mediator.Send(message).GetAwaiter().GetResult();
                Channel.BasicAck(@event.DeliveryTag, false);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error while retrieving message from queue.");
                Channel.BasicNack(@event.DeliveryTag, multiple: false, requeue: false);
            }
        }
    }
}