namespace Booking.Infrastructure.MessageBroker
{
    public sealed class BookingProducer : IProducer
    {
        IPublishEndpoint _publishEndpoint;

        public BookingProducer(
            IPublishEndpoint publishEndpoint)
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