namespace KedaWithRabbitMQSample.Common
{
    public class RabbitMqConfiguration<IRabbitMqItem>
    {
        public string QueueName { get; set; }
        public string QueueExchange { get; set; }
        public string RoutingKey { get; set; }
        public ushort PrefetchCount { get; set; } = 1;
        public string Type { get; set; } = "direct";
        public bool Durable { get; set; } = true;
        public bool AutoDelete { get; set; } = false;
        public bool Exclusive { get; set; } = false;
        public string ContentType { get; } = "application/json";
    }
}