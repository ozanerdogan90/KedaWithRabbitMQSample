using KedaWithRabbitMQSample.Commands;
using KedaWithRabbitMQSample.Common.Producer;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KedaWithRabbitMQSample.Api
{
    public interface IOrderService
    {
        Task Execute(OrderCommand order, CancellationToken token);
    }

    public class OrderService : IOrderService
    {
        private readonly IRabbitMqProducer<OrderCommand> _producer;

        public OrderService(IRabbitMqProducer<OrderCommand> producer)
        {
            _producer = producer;
        }

        public async Task Execute(OrderCommand order, CancellationToken token)
        {
            _producer.Publish(order);
            await Task.Delay(TimeSpan.FromSeconds(1), token);
        }
    }
}