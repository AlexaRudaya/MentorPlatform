using Booking.ApplicationCore.Interfaces.IBackgroundJobs;
using System.Threading;

namespace Booking.Infrastructure.BackGroundJobs
{
    public class BackgroundJobsService : IBackgroundJobsService
    {
        private readonly IProducer _producer;

        public BackgroundJobsService(
            IProducer producer)
        {
            _producer = producer;
        }

        public async Task PublishBookingEvent(MeetingBookingEvent bookingEventToPublish,
            CancellationToken cancellationToken = default)
        {
            await _producer.PublishAsync(bookingEventToPublish, cancellationToken);
        }
    }
}