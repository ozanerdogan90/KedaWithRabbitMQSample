using KedaWithRabbitMQSample.Common;
using MediatR;
using System;

namespace KedaWithRabbitMQSample.Commands
{
    public class OrderCommand : IRabbitMqItem, IRequest<Unit>
    {
        public Guid Id { get; set; }
        public DateTime Created { get; } = DateTime.UtcNow;

        public byte DeliveryMode => 1; //// persistent

        public OrderCommand()
        {
            Id = Guid.NewGuid();
        }
    }
}