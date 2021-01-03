namespace KedaWithRabbitMQSample.Common
{
    public interface IRabbitMqItem
    {
        public byte DeliveryMode { get; } //// 0=> transient , 1=> persist
    }
}