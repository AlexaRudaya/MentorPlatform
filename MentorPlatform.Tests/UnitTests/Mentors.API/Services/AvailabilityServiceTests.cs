using MentorPlatform.Shared.MassTransitEvents;
using MentorPlatform.Shared.MessageBus;
using MentorPlatform.Tests.UnitTests.Mentors.API.BogusData;
using Mentors.ApplicationCore.DTO;
using Mentors.ApplicationCore.Exceptions;
using Mentors.ApplicationCore.Interfaces.IService;
using Mentors.ApplicationCore.Services;
using Mentors.Domain.Abstractions.IRepository;
using Mentors.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using System.Linq.Expressions;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Services
{
    public class AvailabilityServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IAvailabilityRepository> _mockAvailabilityRepository;
        private readonly Mock<ILogger<AvailabilityService>> _mockLogger;
        private readonly Mock<IProducer> _mockProducer;
        private readonly IAvailabilityService _availabilityService;
        private readonly AvailabilityGenerator _availabilityGenerator;

        public AvailabilityServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockAvailabilityRepository = new Mock<IAvailabilityRepository>();
            _mockLogger = new Mock<ILogger<AvailabilityService>>();
            _mockProducer = new Mock<IProducer>();
            _availabilityGenerator = new AvailabilityGenerator();
            _availabilityService = new AvailabilityService(
                _mockAvailabilityRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object,
                _mockProducer.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfAvailabilityDto()
        {
            // Arrange
            var availabilities = new List<Availability>
            {
                _availabilityGenerator.GenerateFakeAvailability(),
                _availabilityGenerator.GenerateFakeAvailability(),
                _availabilityGenerator.GenerateFakeAvailability(),
            };
            var cancellationToken = CancellationToken.None;

            _mockAvailabilityRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<Availability>, IIncludableQueryable<Availability, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(availabilities);

            var availabilitiesDto = availabilities.Select(availability => new AvailabilityDto
            {
                Id = availability.Id,
                Date = availability.Date,
                IsAvailable = availability.IsAvailable,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                MentorId = availability.MentorId
            }).ToList();

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<AvailabilityDto>>(availabilities))
                .Returns(availabilitiesDto);

            // Act
            var result = await _availabilityService.GetAllAsync(cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Should().HaveCount(3);
        }

        [Fact]
        public async Task GetAllAsync_WhenAvailabilitiesAreNull_ShouldThrowAvailabilityNotFoundException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            _mockAvailabilityRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<Availability>, IIncludableQueryable<Availability, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<Availability>)null);

            // Act
            var result = async() => await _availabilityService.GetAllAsync(cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<AvailabilityNotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnAvailabilityDto()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();
            var cancellationToken = CancellationToken.None;

            _mockAvailabilityRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Availability, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(availability);

            var availabilityDto = new AvailabilityDto
            {
                Id = availability.Id,
                Date = availability.Date,
                IsAvailable = availability.IsAvailable,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                MentorId = availability.MentorId
            };

            _mockMapper
                .Setup(mapper => mapper.Map<AvailabilityDto>(availability))
                .Returns(availabilityDto);

            // Act
            var result = await _availabilityService.GetByIdAsync(availability.Id, cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result.Id
                .Should().Be(availability.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WhenAvailabilityIsNull_ShouldThrowAvailabilityNotFoundException()
        {
            // Arrange
            var availabilityId = _availabilityGenerator.GenerateFakeAvailability().Id;
            var cancellationToken = CancellationToken.None;

            _mockAvailabilityRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Availability, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Availability)null);

            // Act
            var result = async() => await _availabilityService.GetByIdAsync(availabilityId, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<AvailabilityNotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedAvailabilityDto()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();
            var cancellationToken = CancellationToken.None;

            var availabilityDto = new AvailabilityDto
            {
                Id = availability.Id,
                Date = availability.Date,
                IsAvailable = availability.IsAvailable,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                MentorId = availability.MentorId
            };

            _mockMapper
                .Setup(mapper => mapper.Map<Availability>(availabilityDto))
                .Returns(availability);

            _mockAvailabilityRepository
                .Setup(repository => repository.CreateAsync(availability, cancellationToken))
                .Returns(Task.CompletedTask);

            _mockMapper
                .Setup(mapper => mapper.Map<AvailabilityDto>(availability))
                .Returns(availabilityDto);

            // Act
            var result = await _availabilityService.CreateAsync(availabilityDto, cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Id.Should().Be(availabilityDto.Id);

            _mockAvailabilityRepository
                .Verify(repository => repository
                    .CreateAsync(availability, cancellationToken),
                    Times.Once);
        }

        [Fact]
        public async Task CreateAsync_ShouldPubishAvailabilityEvent()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();
            var cancellationToken = CancellationToken.None;

            var availabilityDto = new AvailabilityDto
            {
                Id = availability.Id,
                Date = availability.Date,
                IsAvailable = availability.IsAvailable,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                MentorId = availability.MentorId
            };

            var eventToPublish = new AvailabilityOfMentorEvent();

            _mockMapper
                .Setup(mapper => mapper.Map<Availability>(availabilityDto))
                .Returns(availability);
            _mockMapper
                .Setup(mapper => mapper.Map<AvailabilityDto>(availability))
                .Returns(availabilityDto);
            _mockMapper
                .Setup(mapper => mapper.Map<AvailabilityOfMentorEvent>(availability))
                .Returns(eventToPublish);

            _mockProducer
                .Setup(producer => producer.PublishAsync(eventToPublish, cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _availabilityService.CreateAsync(availabilityDto);

            // Assert
            result.Should().NotBeNull();

            _mockProducer
                .Verify(producer => producer
                    .PublishAsync(eventToPublish, cancellationToken),
                Times.Once());
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedAvailability()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();
            var cancellationToken = CancellationToken.None;

            var availabilityDto = new AvailabilityDto
            {
                Id = availability.Id,
                Date = availability.Date,
                IsAvailable = availability.IsAvailable,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                MentorId = availability.MentorId
            };

            _mockAvailabilityRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Availability, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(availability);

            _mockMapper
                .Setup(mapper => mapper.Map<Availability>(availabilityDto))
                .Returns(availability);

            // Act
            var result = await _availabilityService.UpdateAsync(availabilityDto, cancellationToken);

            // Assert
            result
                .Should().NotBeNull();

            _mockAvailabilityRepository
                .Verify(repository => repository
                    .UpdateAsync(availability, cancellationToken),
                    Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenAvailabilityIsNull_ShouldThrowAvailabilityNotFoundException()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();
            var cancellationToken = CancellationToken.None;

            var availabilityDto = new AvailabilityDto
            {
                Id = availability.Id,
                Date = availability.Date,
                IsAvailable = availability.IsAvailable,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                MentorId = availability.MentorId
            };

            _mockAvailabilityRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Availability, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Availability)null);

            // Act
            var result = async() => await _availabilityService.UpdateAsync(availabilityDto, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<AvailabilityNotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteAvailability()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();
            var cancellationToken = CancellationToken.None;

            var availabilityDto = new AvailabilityDto
            {
                Id = availability.Id,
                Date = availability.Date,
                IsAvailable = availability.IsAvailable,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                MentorId = availability.MentorId
            };

            _mockAvailabilityRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Availability, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(availability);

            _mockMapper
                .Setup(mapper => mapper.Map<AvailabilityDto>(availability))
                .Returns(availabilityDto);

            // Act
            var result = await _availabilityService.DeleteAsync(availability.Id);

            // Assert
            result
                .Should().NotBeNull();

            _mockAvailabilityRepository
                .Verify(repository => repository
                    .DeleteAsync(availability, cancellationToken),
                    Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenAvailabilityIsNull_ShouldThrowAvailabilityNotFoundException()
        {
            // Arrange
            var availability = _availabilityGenerator.GenerateFakeAvailability();
            var cancellationToken = CancellationToken.None;

            var availabilityDto = new AvailabilityDto
            {
                Id = availability.Id,
                Date = availability.Date,
                IsAvailable = availability.IsAvailable,
                StartTime = availability.StartTime,
                EndTime = availability.EndTime,
                MentorId = availability.MentorId
            };

            _mockAvailabilityRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Availability, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Availability)null);

            // Act
            var result = async() => await _availabilityService.DeleteAsync(availability.Id, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<AvailabilityNotFoundException>();
        }
    }
}