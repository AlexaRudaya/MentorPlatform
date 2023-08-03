namespace MentorPlatform.Shared.MassTransitEvents
{
    public class AvailabilityOfMentorEvent
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Guid MentorId { get; set; }
    }
}