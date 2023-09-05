namespace MentorPlatform.Tests.UnitTests.Mentors.API.Services
{
    public class CategoryServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<ILogger<CategoryService>> _mockLogger;
        private readonly ICategoryService _categoryService;
        private readonly CategoryGenerator _categoryGenerator;
        private readonly CategoryServiceHelper _helper;
        private readonly CancellationToken _cancellationToken;

        public CategoryServiceTests()
        {
            _mockMapper = new Mock<IMapper>();
            _mockCategoryRepository = new Mock<ICategoryRepository>();
            _mockLogger = new Mock<ILogger<CategoryService>>();
            _categoryGenerator = new CategoryGenerator();
            _categoryService = new CategoryService(
                _mockCategoryRepository.Object,
                _mockMapper.Object,
                _mockLogger.Object);
            _helper = new CategoryServiceHelper(_mockCategoryRepository,
                _mockMapper);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfCategoryDto()
        {
            // Arrange
            var categories = new List<Category>
            {
                _categoryGenerator.GenerateFakeCategory(),
                _categoryGenerator.GenerateFakeCategory(),
                _categoryGenerator.GenerateFakeCategory()
            };

            _helper.SetupGetAllAsync(categories);

            var categoriesDto = _helper.GenerateDtoList(categories);

            _mockMapper
                .Setup(mapper =>mapper.Map<IEnumerable<CategoryDto>>(categories))
                .Returns(categoriesDto);

            // Act
            var result = await _categoryService.GetAllAsync(_cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result 
                .Should().HaveCount(3);
        }

        [Fact]
        public async Task GetAllAsync_WhenCategoriesAreNull_ShouldThrowCategoryNotFoundException()
        {
            // Arrange
            _helper.SetupGetAllAsyncWhenNull();

            // Act
            var result = async() => await _categoryService.GetAllAsync(_cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCategoryDto()
        {
            // Arrange
            var category = _categoryGenerator.GenerateFakeCategory();

            _helper.SetupGetByIdAsync(category);

            var categoryDto = _helper.GenerateDtoFromCategory(category);

            _helper.SetupMapperForCategoryToDto(category, categoryDto);

            // Act
            var result = await _categoryService.GetByIdAsync(category.Id, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result.Id
                .Should().Be(category.Id);
        }

        [Fact]
        public async Task GetByIdAsync_WhenCategoryIsNull_ShouldThrowCategoryNotFoundException()
        {
            // Arrange
            var categoryId = _categoryGenerator.GenerateFakeCategory().Id;
           
            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _categoryService.GetByIdAsync(categoryId, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedCategoryDto()
        {
            // Arrange
            var category = _categoryGenerator.GenerateFakeCategory();

            var categoryDto = _helper.GenerateDtoFromCategory(category);

            _helper.SetupMapperForDtoToCategory(category, categoryDto);

            _helper.SetupCreateAsync(category);

            // Act
            var result = await _categoryService.CreateAsync(categoryDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Id.Should().Be(categoryDto.Id);

            _mockCategoryRepository
                .Verify(repository => repository
                    .CreateAsync(category, _cancellationToken),
                Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedCategoryDto()
        {
            // Arrange
            var category = _categoryGenerator.GenerateFakeCategory();

            var categoryDto = _helper.GenerateDtoFromCategory(category);

            _helper.SetupGetByIdAsync(category);

            _helper.SetupMapperForDtoToCategory(category, categoryDto);

            // Act
            var result = await _categoryService.UpdateAsync(categoryDto, _cancellationToken);

            // Assert
            result
                .Should().NotBeNull();

            _mockCategoryRepository
               .Verify(repository => repository
                   .UpdateAsync(category, _cancellationToken),
               Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryIsNull_ShouldThrowCategoryNotFoundException()
        {
            // Arrange
            var category = _categoryGenerator.GenerateFakeCategory();

            var categoryDto = _helper.GenerateDtoFromCategory(category);

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _categoryService.UpdateAsync(categoryDto, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCategory()
        {
            // Arrange
            var category = _categoryGenerator.GenerateFakeCategory();

            var categoryDto = _helper.GenerateDtoFromCategory(category);

            _helper.SetupGetByIdAsync(category);

            _helper.SetupMapperForCategoryToDto(category, categoryDto);

            // Act
            var result = await _categoryService.DeleteAsync(category.Id);

            // Assert
            result
                .Should().NotBeNull();

            _mockCategoryRepository
               .Verify(repository => repository
                   .DeleteAsync(category, _cancellationToken),
               Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenCategoryIsNull_ShouldThrowCategoryNotFoundException()
        {
            // Arrange
            var category = _categoryGenerator.GenerateFakeCategory();

            var categoryDto = _helper.GenerateDtoFromCategory(category);

            _helper.SetupGetByIdAsyncWhenNull();

            // Act
            var result = async() => await _categoryService.DeleteAsync(category.Id, _cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<CategoryNotFoundException>();
        }
    }
}