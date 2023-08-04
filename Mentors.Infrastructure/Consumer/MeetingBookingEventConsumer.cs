namespace Mentors.Infrastructure.Consumer
{
    public class MeetingBookingEventConsumer : IConsumer<MeetingBookingEvent>
    {
        private readonly ILogger<MeetingBookingEventConsumer> _logger;
        private readonly IMentorRepository _mentorRepository;
        private readonly IAvailabilityRepository _availabilityRepository;

        public MeetingBookingEventConsumer(
            ILogger<MeetingBookingEventConsumer> logger,
            IMentorRepository mentorRepository,
            IAvailabilityRepository availabilityRepository)
        {
            _logger = logger;
            _mentorRepository = mentorRepository;
            _availabilityRepository = availabilityRepository;
        }

        public async Task Consume(ConsumeContext<MeetingBookingEvent> context)
        {
            var meetingBookingEvent = context.Message;

            _logger.LogInformation($"Booking event is received with Id: {meetingBookingEvent.Id}, " +
                $"StartTime: {meetingBookingEvent.StartTimeBooking}" +
                $"EndTime: {meetingBookingEvent.EndTimeBooking}, " +
                $"StudentId: {meetingBookingEvent.StudentId}, " +
                $"MentorId: {meetingBookingEvent.MentorId}");

            await UpdateMentorStatusToBusyAsync(meetingBookingEvent.MentorId, 
                meetingBookingEvent.StartTimeBooking, 
                meetingBookingEvent.EndTimeBooking);
        }

        private async Task UpdateMentorStatusToBusyAsync(string mentorId, DateTime startTimeBooking, 
            DateTime endTimeBooking, CancellationToken cancellationToken = default)
        {
            var mentor = await _mentorRepository.GetOneByAsync(
               include: query => query
                   .Include(mentor => mentor.Category)
                   .Include(mentor => mentor.Availabilities),
               expression: mentor => mentor.Id.Equals(Guid.Parse(mentorId)),
               cancellationToken: cancellationToken);

            if (mentor is null)
            {
                throw new MentorNotFoundException();
            }

            var availabilityToUpdate = mentor.Availabilities
                                             .FirstOrDefault(availability => availability.StartTime == startTimeBooking &&
                                                 availability.EndTime == endTimeBooking);

            if (availabilityToUpdate is null) 
            {
                throw new AvailabilityNotFoundException();
            }

            availabilityToUpdate.IsAvailable = false;

            await _availabilityRepository.UpdateAsync(availabilityToUpdate);

            _logger.LogInformation($"Data for Availability with Id: {availabilityToUpdate.Id} has been updated: IsAvailable is set to false.");
        }
    }
}