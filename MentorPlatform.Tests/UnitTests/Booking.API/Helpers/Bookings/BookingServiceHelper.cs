namespace MentorPlatform.Tests.UnitTests.Booking.API.Helpers.Bookings
{
    public class BookingServiceHelper
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMentorBookingRepository> _mockBookingRepository;
        private readonly Mock<IGetMentorClient> _mockMentorClient;

        public BookingServiceHelper(
            Mock<IMapper> mockMapper,
            Mock<IMentorBookingRepository> mockBookingRepository,
            Mock<IGetMentorClient> mockMentorClient)
        {
            _mockMapper = mockMapper;
            _mockBookingRepository = mockBookingRepository;
            _mockMentorClient = mockMentorClient;
        }

        public void SetupGetAllAsync(List<MentorBooking> bookings)
        {
            _mockBookingRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<MentorBooking>, 
                    IIncludableQueryable<MentorBooking, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookings);
        }

        public void SetupGetAllAsyncWhenNull()
        {
            _mockBookingRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<MentorBooking>, 
                    IIncludableQueryable<MentorBooking, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<MentorBooking>)null);
        }

        public void SetupGetByIdAsync(MentorBooking booking)
        {
            _mockBookingRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<MentorBooking, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(booking);
        }

        public void SetupGetByIdAsyncWhenNull()
        {
            _mockBookingRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<MentorBooking, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((MentorBooking)null);
        }

        public void SetupCreateAsync(MentorBooking booking)
        {
            _mockBookingRepository
                .Setup(repository => repository.CreateAsync(
                    booking,
                    It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        public void SetupGetBookingsForStudent(Guid studentId, 
            List<MentorBooking> bookings)
        {
            _mockBookingRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<MentorBooking>,
                    IIncludableQueryable<MentorBooking, object>>>(),
                    booking => booking.StudentId.Equals(studentId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookings);
        }

        public void SetupGetBookingsForStudentWhenNull(Guid studentId)
        {
            _mockBookingRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<MentorBooking>,
                    IIncludableQueryable<MentorBooking, object>>>(),
                    booking => booking.StudentId.Equals(studentId),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((List<MentorBooking>)null);
        }

        public void SetupGetMentorAsync(MentorDto mentorReply,
            BookingDto bookingDto)
        {
            _mockMentorClient
                .Setup(client => client.GetMentorAsync(
                    bookingDto.MentorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentorReply);
        }

        public void SetupGetMentorAsyncWhenNull(BookingDto bookingDto)
        {
            _mockMentorClient
                .Setup(c => c.GetMentorAsync(
                    bookingDto.MentorId,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((MentorDto)null);
        }

        public void SetupGetBookingsForMentorAsync(List<MentorBooking> bookings)
        {
            _mockBookingRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<MentorBooking>,
                    IIncludableQueryable<MentorBooking, object>>>(),
                    It.IsAny<Expression<Func<MentorBooking, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(bookings);
        }

        public void SetupGetBookingsForMentorAsync()
        {
            _mockBookingRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<MentorBooking>,
                    IIncludableQueryable<MentorBooking, object>>>(),
                    It.IsAny<Expression<Func<MentorBooking, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<MentorBooking>)null);
        }

        public List<BookingDto> GenerateDtoList(IEnumerable<MentorBooking> bookings)
        {
            return bookings.Select(booking => new BookingDto
            {
                Id = booking.Id,
                StartTimeBooking = booking.StartTimeBooking,
                EndTimeBooking = booking.EndTimeBooking,
                StudentId = booking.StudentId,
                MentorId = booking.MentorId
            }).ToList();
        }

        public BookingDto GenerateDtoFromBooking(MentorBooking booking)
        {
            return new BookingDto
            {
                Id = booking.Id,
                StartTimeBooking = booking.StartTimeBooking,
                EndTimeBooking = booking.EndTimeBooking,
                StudentId = booking.StudentId,
                MentorId = booking.MentorId
            };
        }

        public void SetupMapperForBookingToDto(MentorBooking booking,
            BookingDto bookingDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<BookingDto>(booking))
                .Returns(bookingDto);
        }

        public void SetupMapperForBookingsListToDto(IEnumerable<MentorBooking> bookings,
            IEnumerable<BookingDto> bookingsDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<BookingDto>>(bookings))
                .Returns(bookingsDto);
        }

        public void SetupMapperForDtoToBooking(MentorBooking booking,
            BookingDto bookingDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<MentorBooking>(bookingDto))
                .Returns(booking);
        }

        public void SetupMapperForDtoToBookingEvent(MentorBooking booking,
            MeetingBookingEvent eventToPublish)
        {
            _mockMapper
               .Setup(mapper => mapper.Map<MeetingBookingEvent>(booking))
               .Returns(eventToPublish);
        }
    }
}