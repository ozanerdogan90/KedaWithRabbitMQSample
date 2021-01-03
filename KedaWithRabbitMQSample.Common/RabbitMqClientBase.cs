using Microsoft.Extensions.Logging;
using RabbitMQ.Client;
using System;

namespace KedaWithRabbitMQSample.Common
{
    public abstract class RabbitMqClientBase<IRabbitMqItem> : IDisposable
    {
        protected IModel Channel { get; private set; }
        private IConnection _connection;
        private readonly ConnectionFactory _connectionFactory;
        protected readonly ILogger _logger;
        protected readonly RabbitMqConfiguration<IRabbitMqItem> _configuration;

        protected RabbitMqClientBase(
            ConnectionFactory connectionFactory,
            RabbitMqConfiguration<IRabbitMqItem> configuration,
            ILogger logger)
        {
            _connectionFactory = connectionFactory;
            _logger = logger;
            _configuration = configuration;
            ConnectToRabbitMq();
        }

        private void ConnectToRabbitMq()
        {
            if (_connection == null || _connection.IsOpen == false)
            {
                _connection = _connectionFactory.CreateConnection();
            }

            if (Channel == null || Channel.IsOpen == false)
            {
                Channel = _connection.CreateModel();
                Channel.BasicQos(0, _configuration.PrefetchCount, false);
                Channel.ExchangeDeclare(exchange: _configuration.QueueExchange, type: _configuration.Type, durable: _configuration.Durable, autoDelete: _configuration.AutoDelete);
                Channel.QueueDeclare(queue: _configuration.QueueName, durable: _configuration.Durable, exclusive: _configuration.Exclusive, autoDelete: _configuration.AutoDelete);
                Channel.QueueBind(queue: _configuration.QueueName, exchange: _configuration.QueueExchange, routingKey: _configuration.RoutingKey);
            }
        }

        public void Dispose()
        {
            try
            {
                Channel?.Close();
                Channel?.Dispose();
                Channel = null;

                _connection?.Close();
                _connection?.Dispose();
                _connection = null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Cannot dispose RabbitMQ channel or connection");
            }
        }
    }
}