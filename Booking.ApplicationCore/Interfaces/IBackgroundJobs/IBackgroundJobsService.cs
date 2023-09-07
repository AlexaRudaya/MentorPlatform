namespace Booking.ApplicationCore.Interfaces.IBackgroundJobs
{
    public interface IBackgroundJobsService
    {
        Task PublishBookingEvent(MeetingBookingEvent bookingEventToPublish, CancellationToken cancellationToken = default);

        Task<IEnumerable<AvailabilityDto>> GetMentorAvailabilitiesFromMentorApi(string mentorId, CancellationToken cancellationToken = default);

        void EnqueueJob(Expression<Action> methodCall);
    }
}