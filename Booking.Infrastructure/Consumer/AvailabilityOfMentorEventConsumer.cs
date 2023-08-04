namespace Booking.Infrastructure.Consumer
{
    public sealed class AvailabilityOfMentorEventConsumer : IConsumer<AvailabilityOfMentorEvent>
    {
        private readonly ILogger<AvailabilityOfMentorEventConsumer> _logger;

        public AvailabilityOfMentorEventConsumer(
            ILogger<AvailabilityOfMentorEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<AvailabilityOfMentorEvent> context)
        {
            var availabilityEvent = context.Message;

            _logger.LogInformation($"Received Availability event with Id: {availabilityEvent.Id}, " +
                $"Date: {availabilityEvent.Date}, StartTime: {availabilityEvent.StartTime}, " +
                $"EndTime: {availabilityEvent.EndTime}, MentorId: {availabilityEvent.MentorId}");

            return Task.CompletedTask;
        }
    }
}