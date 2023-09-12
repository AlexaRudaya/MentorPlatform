namespace MentorPlatform.Tests.UnitTests.Mentors.API.Services
{
    public class MentorshipSubjectServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<IMentorshipSubjectRepository> _mockRepository;
        private readonly Mock<ILogger<MentorshipSubjectService>> _mockLogger;
        private readonly IMentorshipSubjectService _subjectService;
        private readonly MentorshipSubjectGenerator _subjectGenerator;
        private readonly SubjectServiceHelper _helper;
        private readonly CancellationToken _cancellationToken;

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
            _helper = new SubjectServiceHelper(_mockRepository,
                _mockMapper);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetAllAsync_WhenSubjectsAreFound_ShouldReturnListOfMentorshipSubjectDto()
        {
            // Arrange
            var subjects = new List<MentorshipSubject>
            {
                _subjectGenerator.GenerateFakeMentorshipSubject(),
                _subjectGenerator.GenerateFakeMentorshipSubject(),
                _subjectGenerator.GenerateFakeMentorshipSubject()
            };
            var subjectsDto = _helper.GenerateDtoList(subjects);

            _helper.SetupGetAllAsync(subjects);
            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<MentorshipSubjectDto>>(subjects))
                .Returns(subjectsDto);

            // Act
            var result = await _subjectService.GetAllAsync(_cancellationToken);

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
            _helper.SetupGetAllAsyncWhenNull();

            // Act
            var result = async() => await _subjectService.GetAllAsync(_cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorshipSubjectNotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_WhenSubjectIsFound_ShouldReturnMentorshipSubjectDto()
        {
            // Arrange
            var subject = _subjectGenerator.GenerateFakeMentorshipSubject();
            var subjectDto = _helper.GenerateDtoFromSubject(subject);

            _helper.SetupGetByIdAsync(subject);
            _helper.SetupMapperForSubjectToDto(subject, subjectDto);

            // Act
            var result = await _subjectService.GetByIdAsync(subject.Id, _cancellationToken);

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

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _subjectService.GetByIdAsync(subjectId, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorshipSubjectNotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_WhenModelIsValid_ShouldReturnCreatedMentorshipSubjectDto()
        {
            // Arrange
            var subject = _subjectGenerator.GenerateFakeMentorshipSubject();
            var subjectDto = _helper.GenerateDtoFromSubject(subject);

            _helper.SetupMapperForDtoToSubject(subject, subjectDto);
            _helper.SetupCreateAsync(subject);

            // Act
            var result = await _subjectService.CreateAsync(subjectDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Id.Should().Be(subjectDto.Id);

            _mockRepository
                .Verify(repository => repository
                    .CreateAsync(subject, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenSubjectIsFound_ShouldReturnUpdatedMentorshipSubjectDto()
        {
            // Arrange
            var subject = _subjectGenerator.GenerateFakeMentorshipSubject();
            var subjectDto = _helper.GenerateDtoFromSubject(subject);

            _helper.SetupGetByIdAsync(subject);
            _helper.SetupMapperForDtoToSubject(subject, subjectDto);

            // Act
            var result = await _subjectService.UpdateAsync(subjectDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();

            _mockRepository
               .Verify(repository => repository
                   .UpdateAsync(subject, _cancellationToken),
               Times.Once);
        }


        [Fact]
        public async Task UpdateAsync_WhenSubjectIsNull_ShouldThrowMentorshipSubjectNotFoundException()
        {
            // Arrange
            var subject = _subjectGenerator.GenerateFakeMentorshipSubject();
            var subjectDto = _helper.GenerateDtoFromSubject(subject);

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _subjectService.UpdateAsync(subjectDto, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorshipSubjectNotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_WhenSubjectIsFound_ShouldDeleteMentorshipSubject()
        {
            // Arrange
            var subjectId = _subjectGenerator.GenerateFakeMentorshipSubject().Id;

            // Act
            await _subjectService.DeleteAsync(subjectId, _cancellationToken);

            // Assert
            _mockRepository
                .Verify(repository => repository
                    .DeleteAsync(subjectId, _cancellationToken),
                Times.Once);
        }
    }
}