using AvailabilityDto = Mentors.ApplicationCore.DTO.AvailabilityDto;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Helpers.Availabilities
{
    public class AvailabilityServiceHelper
    {
        private readonly Mock<IAvailabilityRepository> _mockAvailabilityRepository;
        private readonly Mock<IProducer> _mockProducer;
        private readonly Mock<IMapper> _mockMapper;

        public AvailabilityServiceHelper(
             Mock<IAvailabilityRepository> mockAvailabilityRepository,
             Mock<IProducer> mockProducer,
             Mock<IMapper> mockMapper)
        {
            _mockAvailabilityRepository = mockAvailabilityRepository;
            _mockProducer = mockProducer;
            _mockMapper = mockMapper;
        }

        public void SetupGetAllAsync(List<Availability> availabilities)
        {
            _mockAvailabilityRepository
                .Setup(repository => repository.GetAllByAsync(
                     It.IsAny<Func<IQueryable<Availability>, IIncludableQueryable<Availability, object>>>(),
                     null,
                     It.IsAny<CancellationToken>()))
                .ReturnsAsync(availabilities);
        }

        public void SetupGetAllAsyncWhenNull()
        {
            _mockAvailabilityRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<Availability>, IIncludableQueryable<Availability, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<Availability>)null);
        }

        public void SetupGetByIdAsync(Availability availability)
        {
            _mockAvailabilityRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Availability, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(availability);
        }

        public void SetupGetByIdAsyncWhenNull()
        {
            _mockAvailabilityRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Availability, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Availability)null);
        }

        public void SetupCreateAsync(Availability availability)
        {
            _mockAvailabilityRepository
                .Setup(repository => repository.CreateAsync(
                    availability, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        public void SetupPublishAvailabilityEvent(AvailabilityOfMentorEvent eventToPublish)
        {
            _mockProducer
                .Setup(producer => producer.PublishAsync(
                    eventToPublish, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        public void SetupDeleteAsync(Availability availability)
        {
            _mockAvailabilityRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Availability, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(availability);
        }

        public List<AvailabilityDto> GenerateDtoList(IEnumerable<Availability> availabilities)
        {
            return availabilities.Select(availability => new AvailabilityDto
            {
                Id = availability.Id,
                Date = availability.Date,
                IsAvailable = availability.IsAvailable,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                MentorId = availability.MentorId
            }).ToList();
        }

        public AvailabilityDto GenerateDtoFromAvailability(Availability availability)
        {
            return new AvailabilityDto
            {
                Id = availability.Id,
                Date = availability.Date,
                IsAvailable = availability.IsAvailable,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                MentorId = availability.MentorId
            };
        }

        public void SetupMapperForAvailabilityToDto(Availability availability, 
            AvailabilityDto availabilityDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<AvailabilityDto>(availability))
                .Returns(availabilityDto);
        }

        public void SetupMapperForDtoToAvailability(Availability availability,
            AvailabilityDto availabilityDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<Availability>(availabilityDto))
                .Returns(availability);
        }
    }
}