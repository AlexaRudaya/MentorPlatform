namespace Booking.ApplicationCore.Services
{
    public class BookingForMentorService : IBookingForMentorService
    {
        private readonly IMentorBookingRepository _bookingRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingForMentorService> _logger;
        private readonly IBackgroundJobsService _backgroundJobsService;

        public BookingForMentorService(
            IMentorBookingRepository bookingRepository,
            IMapper mapper,
            ILogger<BookingForMentorService> logger,
            IBackgroundJobsService backgroundJobsService)
        {
            _bookingRepository = bookingRepository;
            _mapper = mapper;
            _logger = logger;
            _backgroundJobsService = backgroundJobsService;
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsForMentorAsync(string mentorId,
            CancellationToken cancellationToken = default)
        {
            var bookingsForMentor = await _bookingRepository.GetAllByAsync(expression: booking => booking.MentorId.Equals(mentorId),
                                                                           cancellationToken: cancellationToken);

            if (bookingsForMentor is null)
            {
                throw new BookingNotFoundException();
            }

            var bookingDto = _mapper.Map<IEnumerable<BookingDto>>(bookingsForMentor);

            _logger.LogInformation($"Bookings for a mentor:{mentorId} are loaded.");

            return bookingDto;
        }

        public async Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesOfMentor(string mentorId, 
            CancellationToken cancellationToken = default)
        {
            _backgroundJobsService.ScheduleRecurringJob<IBackgroundJobsService>(
                $"GetAvailabilitiesForMentor-{mentorId}",
                 backgroundJob => backgroundJob.GetMentorAvailabilitiesFromMentorApi(mentorId, cancellationToken), 
                 Cron.Daily());

            var availabilities = await _backgroundJobsService.GetMentorAvailabilitiesFromMentorApi(mentorId, cancellationToken);

            return availabilities;
        }
    }
}