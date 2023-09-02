using MentorPlatform.Tests.UnitTests.Mentors.API.BogusData;
using Mentors.ApplicationCore.DTO;
using Mentors.ApplicationCore.Exceptions;
using Mentors.ApplicationCore.Interfaces.IMongoService;
using Mentors.ApplicationCore.Services.MongoServices;
using Mentors.Domain.Abstractions.IRepository.IMongoRepository;
using Mentors.Domain.Entities.MongoDb;
using Moq;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Services
{
    public class MentorshipSubjectServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMentorshipSubjectRepository> _mockRepository;
        private readonly Mock<ILogger<MentorshipSubjectService>> _mockLogger;
        private readonly IMentorshipSubjectService _subjectService;
        private readonly MentorshipSubjectGenerator _subjectGenerator;

        public MentorshipSubjectServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockRepository = new Mock<IMentorshipSubjectRepository>();
            _mockLogger = new Mock<ILogger<MentorshipSubjectService>>();
            _subjectGenerator = new MentorshipSubjectGenerator();
            _subjectService = new MentorshipSubjectService(
                _mockRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object);
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfMentorshipSubjectDto()
        {
            // Arrange
            var subjects = new List<MentorshipSubject>
            {
                _subjectGenerator.GenerateFakeMentorshipSubject(),
                _subjectGenerator.GenerateFakeMentorshipSubject(),
                _subjectGenerator.GenerateFakeMentorshipSubject()
            };
            var cancellationToken = CancellationToken.None;

            _mockRepository
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(subjects);

            var subjectsDto = subjects.Select(subject => new MentorshipSubjectDto
            {
                Id = subject.Id,
                Name = subject.Name
            }).ToList();

            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<MentorshipSubjectDto>>(subjects))
                .Returns(subjectsDto);

            // Act
            var result = await _subjectService.GetAllAsync(cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Should().HaveCount(3);
        }

        [Fact]
        public async Task GetAllAsync_WhenSubjectsAreNull_ShouldThrowMentorshipSubjectNotFoundException()
        {
            // Arrange
            var cancellationToken = CancellationToken.None;

            _mockRepository
                .Setup(repository => repository.GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<MentorshipSubject>)null);

            // Act
            var result = async() => await _subjectService.GetAllAsync(cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorshipSubjectNotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnMentorshipSubjectDto()
        {
            // Arrange
            var subject = _subjectGenerator.GenerateFakeMentorshipSubject();
            var cancellationToken = CancellationToken.None;

            _mockRepository
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(subject);

            var subjectDto = new MentorshipSubjectDto
            {
                Id = subject.Id,
                Name = subject.Name
            };

            _mockMapper
                .Setup(mapper => mapper.Map<MentorshipSubjectDto>(subject))
                .Returns(subjectDto);

            // Act
            var result = await _subjectService.GetByIdAsync(subject.Id, cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result.Id
                .Should().Be(subject.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WhenSubjectIsNull_ShouldThrowMentorshipSubjectNotFoundException()
        {
            // Arrange
            var subjectId = _subjectGenerator.GenerateFakeMentorshipSubject().Id;
            var cancellationToken = CancellationToken.None;

            _mockRepository
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((MentorshipSubject)null);

            // Act
            var result = async() => await _subjectService.GetByIdAsync(subjectId, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorshipSubjectNotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedMentorshipSubjectDto()
        {
            // Arrange
            var subject = _subjectGenerator.GenerateFakeMentorshipSubject();
            var cancellationToken = CancellationToken.None;

            var subjectDto = new MentorshipSubjectDto
            {
                Id = subject.Id,
                Name = subject.Name
            };

            _mockMapper
                .Setup(mapper => mapper.Map<MentorshipSubject>(subjectDto))
                .Returns(subject);

            _mockRepository
                .Setup(repository => repository.CreateAsync(subject, cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _subjectService.CreateAsync(subjectDto, cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Id.Should().Be(subjectDto.Id);

            _mockRepository
                .Verify(repository => repository
                    .CreateAsync(subject, cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedMentorshipSubjectDto()
        {
            // Arrange
            var subject = _subjectGenerator.GenerateFakeMentorshipSubject();
            var cancellationToken = CancellationToken.None;

            var subjectDto = new MentorshipSubjectDto
            {
                Id = subject.Id,
                Name = subject.Name
            };

            _mockRepository
                .Setup(repository => repository.GetByIdAsync(
                   It.IsAny<string>(),
                   It.IsAny<CancellationToken>()))
                .ReturnsAsync(subject);

            _mockMapper
                .Setup(mapper => mapper.Map<MentorshipSubject>(subjectDto))
                .Returns(subject);

            // Act
            var result = await _subjectService.UpdateAsync(subjectDto, cancellationToken);

            // Assert
            result
                .Should().NotBeNull();

            _mockRepository
               .Verify(repository => repository
                   .UpdateAsync(subject, cancellationToken),
               Times.Once);
        }


        [Fact]
        public async Task UpdateAsync_WhenSubjectIsNull_ShouldThrowMentorshipSubjectNotFoundException()
        {
            // Arrange
            var subject = _subjectGenerator.GenerateFakeMentorshipSubject();
            var cancellationToken = CancellationToken.None;

            var subjectDto = new MentorshipSubjectDto
            {
                Id = subject.Id,
                Name = subject.Name
            };

            _mockRepository
                .Setup(repository => repository.GetByIdAsync(
                    It.IsAny<string>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((MentorshipSubject)null);

            // Act
            var result = async() => await _subjectService.UpdateAsync(subjectDto, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorshipSubjectNotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteMentorshipSubject()
        {
            // Arrange
            var subjectId = _subjectGenerator.GenerateFakeMentorshipSubject().Id;
            var cancellationToken = CancellationToken.None;

            // Act
            await _subjectService.DeleteAsync(subjectId, cancellationToken);

            // Assert
            _mockRepository
                .Verify(repository => repository
                    .DeleteAsync(subjectId, cancellationToken),
                Times.Once);
        }
    }
}