namespace Booking.ApplicationCore.Interfaces.IBackgroundJobs
{
    public interface IBackgroundJobsService
    {
        Task PublishBookingEvent(MeetingBookingEvent bookingEventToPublish, CancellationToken cancellationToken = default);
    }
}