namespace MentorPlatform.Shared.MassTransitEvents
{
    public class MeetingBookingEvent
    {
        public Guid Id { get; set; }

        public DateTime StartTimeBooking { get; set; }

        public DateTime EndTimeBooking { get; set; }

        public Guid StudentId { get; set; }

        public string MentorId { get; set; }
    }
}