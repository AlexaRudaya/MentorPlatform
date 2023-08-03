using MassTransit;
using MentorPlatform.Shared.MassTransitEvents;
using Microsoft.Extensions.Logging;

namespace Mentors.Infrastructure.Consumer
{
    public class MeetingBookingEventConsumer : IConsumer<MeetingBookingEvent>
    {
        private readonly ILogger<MeetingBookingEventConsumer> _logger;

        public MeetingBookingEventConsumer(
            ILogger<MeetingBookingEventConsumer> logger)
        {
            _logger = logger;
        }

        public Task Consume(ConsumeContext<MeetingBookingEvent> context)
        {
            var meetingBookingEvent = context.Message;

            _logger.LogInformation($"Booking is received event with Id: {meetingBookingEvent.Id}, " +
                $"StartTime: {meetingBookingEvent.StartTimeBooking}" +
                $"EndTime: {meetingBookingEvent.EndTimeBooking}, StudentId: {meetingBookingEvent.StudentId}, " +
                $"MentorId: {meetingBookingEvent.MentorId}");

            return Task.CompletedTask;
        }
    }
}