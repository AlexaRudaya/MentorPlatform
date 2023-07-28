namespace Booking.ApplicationCore.Services
{
    public class BookingsService : IBookingsService
    {
        private readonly IBookingsRepository _bookingsRepository;
        private readonly IStudentRepository _studentRepository;
        private readonly IGetMentorClient _mentorClient;
        private readonly IMapper _mapper;
        private readonly ILogger<BookingsService> _logger;

        public BookingsService(
            IBookingsRepository bookingsRepository,
            IStudentRepository studentRepository,
            IGetMentorClient mentorClient,
            IMapper mapper,
            ILogger<BookingsService> logger)
        {
            _bookingsRepository = bookingsRepository;
            _studentRepository = studentRepository;
            _mentorClient = mentorClient;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<BookingsDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            var allBookings = await _bookingsRepository.GetAllByAsync();

            var bookingsDto = _mapper.Map<IEnumerable<BookingsDto>>(allBookings);

            if (allBookings is null)
            {
                throw new BookingNotFoundException();
            }

            _logger.LogInformation("Bookings are loaded");

            return bookingsDto;
        }

        public async Task<BookingsDto> GetByIdAsync(Guid bookingId, 
            CancellationToken cancellationToken = default)
        {
            var booking = await _bookingsRepository.GetOneByAsync(expression: booking => booking.Id.Equals(bookingId),
                                                                  cancellationToken: cancellationToken);

            if (booking is null)
            {
                throw new BookingNotFoundException(bookingId);
            }

            var bookingDto = _mapper.Map<BookingsDto>(booking);

            return bookingDto;
        }

        public async Task<IEnumerable<BookingsDto>> GetBookingsForStudentAsync(Guid studentId,
            CancellationToken cancellationToken = default)
        {
            var bookingsForStudent = await _bookingsRepository.GetAllByAsync(expression: booking => booking.StudentId.Equals(studentId),
                                                                             cancellationToken: cancellationToken);

            if (bookingsForStudent is null)
            {
                throw new BookingNotFoundException();
            }

            var bookingDto = _mapper.Map<IEnumerable<BookingsDto>>(bookingsForStudent);

            return bookingDto;
        }

        public async Task<BookingsDto> CreateAsync(BookingsDto bookingsDto,
            CancellationToken cancellationToken = default)
        {
            var bookingToCreate = _mapper.Map<Bookings>(bookingsDto);

            var student = await _studentRepository.GetOneByAsync(expression: student => student.Id.Equals(bookingsDto.StudentId));

            if (student is null)
            {
                _logger.LogError($"Failed finding student with Id:{bookingToCreate.StudentId}.");
                throw new StudentNotFoundException(bookingToCreate.StudentId);
            }

            bookingToCreate.StudentId = student.Id;

            var mentorReply = await _mentorClient.GetMentorAsync(bookingsDto.MentorId);

            if (mentorReply is null)
            {
                _logger.LogError($"Failed finding mentor with Id:{bookingToCreate.MentorId}.");
                throw new ObjectNotFoundException("Mentor was not found");
            }

            bookingToCreate.MentorId = mentorReply.MentorId;

            await _bookingsRepository.CreateAsync(bookingToCreate, cancellationToken);

            _logger.LogInformation($"Booking with Id: {bookingToCreate.Id}");

            return bookingsDto;
        }

        public async Task<BookingsDto> UpdateAsync(BookingsDto bookingDto,
            CancellationToken cancellationToken = default)
        {
            var existingBooking = await _bookingsRepository.GetOneByAsync(expression: booking => booking.Id.Equals(bookingDto.Id));

            if (existingBooking is null)
            {
                _logger.LogError($"Failed finding booking with Id:{bookingDto.Id} while updating entity.");
                throw new BookingNotFoundException(bookingDto.Id);
            }

            var bookingToUpdate = _mapper.Map<Bookings>(bookingDto);

            await _bookingsRepository.UpdateAsync(bookingToUpdate, cancellationToken);

            _logger.LogInformation($"Data for Booking with Id: {existingBooking.Id} has been successfully updated.");

            return bookingDto;
        }

        public async Task<BookingsDto> DeleteAsync(Guid bookingId, CancellationToken cancellationToken = default)
        {
            var bookingToDelete = await _bookingsRepository.GetOneByAsync(expression: booking => booking.Id.Equals(bookingId));

            if (bookingToDelete is null)
            {
                _logger.LogError($"Failed finding booking with Id:{bookingId} while deleting entity.");
                throw new BookingNotFoundException(bookingId);
            }

            var bookingDeleted = _mapper.Map<BookingsDto>(bookingToDelete);

            await _bookingsRepository.DeleteAsync(bookingToDelete, cancellationToken);

            _logger.LogInformation($"Booking with Id: {bookingId} is removed");

            return bookingDeleted;
        }
    }
}