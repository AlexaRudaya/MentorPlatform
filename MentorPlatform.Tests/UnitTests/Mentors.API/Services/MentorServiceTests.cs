using MentorPlatform.Tests.UnitTests.Mentors.API.BogusData;
using Mentors.ApplicationCore.DTO;
using Mentors.ApplicationCore.Exceptions;
using Mentors.ApplicationCore.Interfaces.IService;
using Mentors.ApplicationCore.Services;
using Mentors.Domain.Abstractions.IRepository;
using Mentors.Domain.Entities;
using Microsoft.EntityFrameworkCore.Query;
using Moq;
using NSubstitute;
using System.Linq.Expressions;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Services
{
    public class MentorServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMentorRepository> _mockMentorRepository;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<ILogger<MentorService>> _mockLogger;
        private readonly IMentorService _mentorService;
        private readonly MentorGenerator _mentorGenerator;

        public MentorServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockMentorRepository = new Mock<IMentorRepository>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockLogger = new Mock<ILogger<MentorService>>();
            _mentorService = new MentorService(
                _mockMentorRepository.Object,
                _mockCategoryRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object);

            var categoryGenerator = new CategoryGenerator();
            var availabilityGenerator = new AvailabilityGenerator();
            _mentorGenerator = new MentorGenerator(categoryGenerator, availabilityGenerator);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfMentorsDto()
        {
            // Arrange
            var mentors = new List<Mentor>
            {
                _mentorGenerator.GenerateFakeMentor(),
                _mentorGenerator.GenerateFakeMentor(),
                _mentorGenerator.GenerateFakeMentor()
            };
            var cancellationToken = CancellationToken.None;

            _mockMentorRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<Mentor>, IIncludableQueryable<Mentor, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentors);

            var mentorsDto = mentors.Select(mentor => new MentorDto
            {
                Id = mentor.Id,
                Name = mentor.Name

            }).ToList();

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<MentorDto>>(mentors))
                .Returns(mentorsDto);

            // Act
            var result = await _mentorService.GetAllAsync(cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Should().HaveCount(3);
        }

        [Fact]
        public async Task GetAllAsync_WhenMentorsAreNull_ShouldThrowMentorNotFoundException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            _mockMentorRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<Mentor>, IIncludableQueryable<Mentor, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<Mentor>)null);

            // Act
            var result = async() => await _mentorService.GetAllAsync(cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorNotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMentorDto()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var cancellationToken = CancellationToken.None;

            _mockMentorRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Mentor, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentor);

            var mentorDto = new MentorDto
            {
                Id = mentor.Id,
                Name = mentor.Name
            };

            _mockMapper
                .Setup(mapper => mapper.Map<MentorDto>(mentor))
                .Returns(mentorDto);

            // Act
            var result = await _mentorService.GetByIdAsync(mentor.Id, cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result.Id
                .Should().Be(mentor.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WhenMentorIsNull_ShouldThrowMentorNotFoundException()
        {
            // Arrange
            var mentorId = _mentorGenerator.GenerateFakeMentor().Id;
            var cancellationToken = CancellationToken.None;

            _mockMentorRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Mentor, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Mentor)null);

            // Act
            var result = async () => await _mentorService.GetByIdAsync(mentorId, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorNotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedMentorDto()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var category = mentor.Category;
            var cancellationToken = CancellationToken.None;

            var mentorDto = new MentorCreateDto
            {
                Id = mentor.Id,
                Name = mentor.Name
            };

            _mockCategoryRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            _mockMapper
               .Setup(mapper => mapper.Map<Mentor>(mentorDto))
               .Returns(mentor);

            _mockMentorRepository
                .Setup(repository => repository.CreateAsync(
                    mentor, cancellationToken))
                .Returns(Task.CompletedTask);

            _mockMapper
                .Setup(mapper => mapper.Map<MentorCreateDto>(mentor))
                .Returns(mentorDto);

            // Act
            var result = await _mentorService.CreateAsync(mentorDto, cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Id.Should().Be(mentorDto.Id);

            _mockMentorRepository
                .Verify(repository => repository
                    .CreateAsync(mentor, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenCategoryIsNull_ShouldThrowCategoryNotFoundException()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var cancellationToken = CancellationToken.None;

            var mentorDto = new MentorCreateDto
            {
                Id = mentor.Id,
                Name = mentor.Name
            };

            _mockCategoryRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category)null);

            // Act
            var result = async() => await _mentorService.CreateAsync(mentorDto, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedMentorDto()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var cancellationToken = CancellationToken.None;

            var mentorDto = new MentorDto
            {
                Id = mentor.Id,
                Name = mentor.Name
            };

            _mockMentorRepository
               .Setup(repository => repository.GetOneByAsync(
                   null,
                   It.IsAny<Expression<Func<Mentor, bool>>>(),
                   It.IsAny<CancellationToken>()))
               .ReturnsAsync(mentor);

            _mockMapper
                .Setup(mapper => mapper.Map<Mentor>(mentorDto))
                .Returns(mentor);

            // Act
            var result = await _mentorService.UpdateAsync(mentorDto, cancellationToken);

            // Assert
            result
                .Should().NotBeNull();

            _mockMentorRepository
                .Verify(repository => repository
                   .UpdateAsync(mentor, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenMentorIsNull_ShouldThrowMentorNotFoundException()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var cancellationToken = CancellationToken.None;

            var mentorDto = new MentorDto
            {
                Id = mentor.Id,
                Name = mentor.Name
            };

            _mockMentorRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Mentor, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Mentor)null);

            // Act
            var result = async() => await _mentorService.UpdateAsync(mentorDto, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorNotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteMentor()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var cancellationToken = CancellationToken.None;

            var mentorDto = new MentorDto
            {
                Id = mentor.Id,
                Name = mentor.Name
            };

            _mockMentorRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Mentor, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(mentor);

            _mockMapper
                .Setup(mapper => mapper.Map<MentorDto>(mentor))
                .Returns(mentorDto);

            // Act
            var result = await _mentorService.DeleteAsync(mentor.Id);

            // Assert
            result
                .Should().NotBeNull();

            _mockMentorRepository
                .Verify(repository => repository
                   .DeleteAsync(mentor, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenMentorIsNull_ShouldThrowMentorNotFoundException()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var cancellationToken = CancellationToken.None;

            var mentorDto = new MentorDto
            {
                Id = mentor.Id,
                Name = mentor.Name
            };

            _mockMentorRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Mentor, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Mentor)null);

            // Act
            var result = async() => await _mentorService.DeleteAsync(mentor.Id, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorNotFoundException>();
        }
    }
}