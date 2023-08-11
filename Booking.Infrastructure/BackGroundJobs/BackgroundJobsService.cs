using Booking.ApplicationCore.Exceptions;
using Booking.ApplicationCore.Interfaces.IBackgroundJobs;

namespace Booking.Infrastructure.BackGroundJobs
{
    public class BackgroundJobsService : IBackgroundJobsService
    {
        private readonly IProducer _producer;
        private readonly IGetMentorClient _mentorClient;
        private readonly IMapper _mapper;
        private readonly ILogger<BackgroundJobsService> _logger;

        public BackgroundJobsService(
            IProducer producer,
            IGetMentorClient mentorClient,
            IMapper mapper,
            ILogger<BackgroundJobsService> logger)
        {
            _producer = producer;
            _mentorClient = mentorClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task PublishBookingEvent(MeetingBookingEvent bookingEventToPublish,
            CancellationToken cancellationToken = default)
        {
            await _producer.PublishAsync(bookingEventToPublish, cancellationToken);
        }

        public async Task<IEnumerable<AvailabilityDto>> GetMentorAvailabilitiesFromMentorApi(string mentorId, 
            CancellationToken cancellationToken = default)
        {
            var mentorReply = await _mentorClient.GetMentorAsync(mentorId);

            if (mentorReply is null)
            {
                _logger.LogError($"Failed finding mentor with Id:{mentorId}");
                throw new ObjectNotFoundException("Mentor was not found");
            }

            var availabilities =_mapper.Map<IEnumerable<AvailabilityDto>>(mentorReply.Availabilities);

            return availabilities;
        }
    }
}