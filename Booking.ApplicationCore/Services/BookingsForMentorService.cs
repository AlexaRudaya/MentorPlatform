namespace Booking.ApplicationCore.Services
{
    public class BookingsForMentorService : IBookingsForMentorService
    {
        private readonly IBookingsRepository _bookingsRepository;
        private readonly IGetMentorClient _mentorClient;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingsService> _logger;

        public BookingsForMentorService(
            IBookingsRepository bookingsRepository,
            IGetMentorClient mentorClient,
            IMapper mapper,
            ILogger<BookingsService> logger)
        {
            _bookingsRepository = bookingsRepository;
            _mentorClient = mentorClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BookingsDto>> GetBookingsForMentorAsync(string mentorId,
            CancellationToken cancellationToken = default)
        {
            var bookingsForMentor = await _bookingsRepository.GetAllByAsync(expression: booking => booking.MentorId.Equals(mentorId),
                                                                            cancellationToken: cancellationToken);

            if (bookingsForMentor is null)
            {
                throw new BookingNotFoundException();
            }

            var bookingDto = _mapper.Map<IEnumerable<BookingsDto>>(bookingsForMentor);

            _logger.LogInformation($"Bookings for a mentor:{mentorId} are loaded.");

            return bookingDto;
        }

        public async Task<IEnumerable<AvailabilityDto>> GetAvailabilitiesOfMentor(string mentorId, 
            CancellationToken cancellationToken = default)
        {
            var mentorReply = await _mentorClient.GetMentorAsync(mentorId);

            if (mentorReply is null)
            {
                _logger.LogError($"Failed finding mentor with Id:{mentorId}");
                throw new ObjectNotFoundException("Mentor was not found");
            }

            var availabilities = _mapper.Map<IEnumerable<AvailabilityDto>>(mentorReply.Availabilities);

            return availabilities;
        }
    }
}