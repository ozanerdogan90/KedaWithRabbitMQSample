namespace KedaWithRabbitMQSample.Common.Producer
{
    public interface IRabbitMqProducer<in T>
    {
        void Publish(T @event);
    }
}