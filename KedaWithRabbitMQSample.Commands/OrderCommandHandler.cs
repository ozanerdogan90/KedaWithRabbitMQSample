using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace KedaWithRabbitMQSample.Commands
{
    public class OrderCommandHandler : IRequestHandler<OrderCommand>
    {
        private readonly ILogger _logger;
        private readonly Random _randomGenerator = new Random(Guid.NewGuid().GetHashCode());

        public OrderCommandHandler(ILogger logger) => _logger = logger;

        public async Task<Unit> Handle(OrderCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Message received {Id}", request.Id);

            int delayMilliSeconds = _randomGenerator.Next(
                    (int)TimeSpan.FromSeconds(0.5).TotalMilliseconds,
                    (int)TimeSpan.FromSeconds(5).TotalMilliseconds
                );

            await Task.Delay(delayMilliSeconds, cancellationToken).ConfigureAwait(false);
            return Unit.Value;
        }
    }
}