namespace Mentors.ApplicationCore.MassTransitEvents
{
    public  class AvailabilityOfMentorCreatedEvent
    {
        public Guid Id { get; set; }

        public DateTime Date { get; set; }

        public DateTime StartTime { get; set; }

        public DateTime EndTime { get; set; }

        public Guid MentorId { get; set; }
    }
}