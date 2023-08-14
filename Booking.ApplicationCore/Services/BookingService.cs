namespace Booking.ApplicationCore.Services
{
    public class BookingService : IBookingService
    {
        private readonly IMentorBookingRepository _bookingRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGetMentorClient _mentorClient;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingService> _logger;
        private readonly IBackgroundJobsService _backgroundJobsService;

        public BookingService(
            IMentorBookingRepository bookingRepository,
            IStudentRepository studentRepository,
            IGetMentorClient mentorClient,
            IMapper mapper,
            ILogger<BookingService> logger,
            IBackgroundJobsService backgroundJobsService)
        {
            _bookingRepository = bookingRepository;
            _studentRepository = studentRepository;
            _mentorClient = mentorClient;
            _mapper = mapper;
            _logger = logger;
            _backgroundJobsService = backgroundJobsService;
        }

        public async Task<IEnumerable<BookingDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var allBookings = await _bookingRepository.GetAllByAsync();

            var bookingsDto = _mapper.Map<IEnumerable<BookingDto>>(allBookings);

            if (allBookings is null)
            {
                throw new BookingNotFoundException();
            }

            _logger.LogInformation("Bookings are loaded");

            return bookingsDto;
        }

        public async Task<BookingDto> GetByIdAsync(Guid bookingId, 
            CancellationToken cancellationToken = default)
        {
            var booking = await _bookingRepository.GetOneByAsync(expression: booking => booking.Id.Equals(bookingId),
                                                                 cancellationToken: cancellationToken);

            if (booking is null)
            {
                throw new BookingNotFoundException(bookingId);
            }

            var bookingDto = _mapper.Map<BookingDto>(booking);

            return bookingDto;
        }

        public async Task<IEnumerable<BookingDto>> GetBookingsForStudentAsync(Guid studentId,
            CancellationToken cancellationToken = default)
        {
            var bookingsForStudent = await _bookingRepository.GetAllByAsync(expression: booking => booking.StudentId.Equals(studentId),
                                                                            cancellationToken: cancellationToken);

            if (bookingsForStudent is null)
            {
                throw new BookingNotFoundException();
            }

            var bookingDto = _mapper.Map<IEnumerable<BookingDto>>(bookingsForStudent);

            return bookingDto;
        }

        public async Task<BookingDto> CreateAsync(BookingDto bookingDto,
            CancellationToken cancellationToken = default)
        {
            var bookingToCreate = _mapper.Map<MentorBooking>(bookingDto);

            var student = await _studentRepository.GetOneByAsync(expression: student => student.Id.Equals(bookingDto.StudentId));

            if (student is null)
            {
                _logger.LogError($"Failed finding student with Id:{bookingToCreate.StudentId}.");
                throw new StudentNotFoundException(bookingToCreate.StudentId);
            }

            bookingToCreate.StudentId = student.Id;

            var mentorReply = await _mentorClient.GetMentorAsync(bookingDto.MentorId);

            if (mentorReply is null)
            {
                _logger.LogError($"Failed finding mentor with Id:{bookingToCreate.MentorId}.");
                throw new ObjectNotFoundException("Mentor was not found");
            }

            bookingToCreate.MentorId = mentorReply.MentorId;

            await _bookingRepository.CreateAsync(bookingToCreate, cancellationToken);

            _logger.LogInformation($"Booking with Id: {bookingToCreate.Id}");

            var eventToPublish = _mapper.Map<MeetingBookingEvent>(bookingToCreate);

            BackgroundJob.Enqueue(() => _backgroundJobsService.PublishBookingEvent(eventToPublish, cancellationToken));

            return bookingDto;
        }

        public async Task<BookingDto> UpdateAsync(BookingDto bookingDto,
            CancellationToken cancellationToken = default)
        {
            var existingBooking = await _bookingRepository.GetOneByAsync(expression: booking => booking.Id.Equals(bookingDto.Id));

            if (existingBooking is null)
            {
                _logger.LogError($"Failed finding booking with Id:{bookingDto.Id} while updating entity.");
                throw new BookingNotFoundException(bookingDto.Id);
            }

            var bookingToUpdate = _mapper.Map<MentorBooking>(bookingDto);

            await _bookingRepository.UpdateAsync(bookingToUpdate, cancellationToken);

            _logger.LogInformation($"Data for Booking with Id: {existingBooking.Id} has been successfully updated.");

            return bookingDto;
        }

        public async Task<BookingDto> DeleteAsync(Guid bookingId, CancellationToken cancellationToken = default)
        {
            var bookingToDelete = await _bookingRepository.GetOneByAsync(expression: booking => booking.Id.Equals(bookingId));

            if (bookingToDelete is null)
            {
                _logger.LogError($"Failed finding booking with Id:{bookingId} while deleting entity.");
                throw new BookingNotFoundException(bookingId);
            }

            var bookingDeleted = _mapper.Map<BookingDto>(bookingToDelete);

            await _bookingRepository.DeleteAsync(bookingToDelete, cancellationToken);

            _logger.LogInformation($"Booking with Id: {bookingId} is removed");

            return bookingDeleted;
        }
    }
}