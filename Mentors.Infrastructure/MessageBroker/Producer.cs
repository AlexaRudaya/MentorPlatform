using MassTransit;
using Mentors.ApplicationCore.Interfaces.IProducer;

namespace Mentors.Infrastructure.MessageBroker
{
    public sealed class Producer : IProducer
    {
        IPublishEndpoint _publishEndpoint;

        public Producer(IPublishEndpoint publishEndpoint)
        {
            _publishEndpoint = publishEndpoint;    
        }

        public Task PublishAsync<T>(T message, CancellationToken cancellationToken = default)
            where T : class
        {
            return _publishEndpoint.Publish(message, cancellationToken);
        }
    }
}