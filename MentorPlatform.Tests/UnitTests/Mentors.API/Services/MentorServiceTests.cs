using MentorDto = Mentors.ApplicationCore.DTO.MentorDto;

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
        private readonly MentorServiceHelper _helper;
        private readonly CategoryServiceHelper _categoryHelper;
        private readonly CancellationToken _cancellationToken;

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
            _helper = new MentorServiceHelper(_mockMentorRepository,
                _mockMapper);
            _categoryHelper = new CategoryServiceHelper(_mockCategoryRepository,
                _mockMapper);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetAllAsync_WhenMentorsAreFound_ShouldReturnListOfMentorsDto()
        {
            // Arrange
            var mentors = new List<Mentor>
            {
                _mentorGenerator.GenerateFakeMentor(),
                _mentorGenerator.GenerateFakeMentor(),
                _mentorGenerator.GenerateFakeMentor()
            };
            var mentorsDto = _helper.GenerateDtoList(mentors);

            _helper.SetupGetAllAsync(mentors);
            _mockMapper
                .Setup(mapper => mapper.Map<IEnumerable<MentorDto>>(mentors))
                .Returns(mentorsDto);

            // Act
            var result = await _mentorService.GetAllAsync(_cancellationToken);

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
            _helper.SetupGetAllAsyncWhenNull();

            // Act
            var result = async() => await _mentorService.GetAllAsync(_cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorNotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_WhenMentorIsFound_ShouldReturnMentorDto()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var mentorDto = _helper.GenerateDtoFromMentor(mentor);

            _helper.SetupGetByIdAsync(mentor);
            _helper.SetupMapperForMentorToDto(mentor, mentorDto);

            // Act
            var result = await _mentorService.GetByIdAsync(mentor.Id, _cancellationToken);

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

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async () => await _mentorService.GetByIdAsync(mentorId, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorNotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_WhenModelIsValid_ShouldReturnCreatedMentorDto()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var category = mentor.Category;
            var mentorDto = _helper.GenerateCreateDtoFromMentor(mentor);

            _categoryHelper.SetupGetByIdAsync(category);
            _helper.SetupMapperForCreateDtoToMentor(mentor, mentorDto);
            _helper.SetupCreateAsync(mentor);
            _helper.SetupMapperForMentorToCreateDto(mentor, mentorDto);

            // Act
            var result = await _mentorService.CreateAsync(mentorDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Id.Should().Be(mentorDto.Id);

            _mockMentorRepository
                .Verify(repository => repository
                    .CreateAsync(mentor, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task CreateAsync_WhenCategoryIsNull_ShouldThrowCategoryNotFoundException()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var mentorDto = _helper.GenerateCreateDtoFromMentor(mentor);

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _mentorService.CreateAsync(mentorDto, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task UpdateAsync_WhenMentorIsFound_ShouldReturnUpdatedMentorDto()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var mentorDto = _helper.GenerateDtoFromMentor(mentor);

            _helper.SetupGetByIdAsync(mentor);
            _helper.SetupMapperForDtoToMentor(mentor, mentorDto);

            // Act
            var result = await _mentorService.UpdateAsync(mentorDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();

            _mockMentorRepository
                .Verify(repository => repository
                   .UpdateAsync(mentor, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenMentorIsNull_ShouldThrowMentorNotFoundException()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var mentorDto = _helper.GenerateDtoFromMentor(mentor);

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _mentorService.UpdateAsync(mentorDto, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorNotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_WhenMentorIsFound_ShouldDeleteMentor()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var mentorDto = _helper.GenerateDtoFromMentor(mentor);

            _helper.SetupGetByIdAsync(mentor);
            _helper.SetupMapperForMentorToDto(mentor, mentorDto);

            // Act
            var result = await _mentorService.DeleteAsync(mentor.Id);

            // Assert
            result
                .Should().NotBeNull();

            _mockMentorRepository
                .Verify(repository => repository
                   .DeleteAsync(mentor, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenMentorIsNull_ShouldThrowMentorNotFoundException()
        {
            // Arrange
            var mentor = _mentorGenerator.GenerateFakeMentor();
            var mentorDto = _helper.GenerateDtoFromMentor(mentor);

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _mentorService.DeleteAsync(mentor.Id, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<MentorNotFoundException>();
        }
    }
}