namespace MentorPlatform.Tests.UnitTests.Mentors.API.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoriesController _controller;
        private readonly CategoryGenerator _categoryData;
        private readonly CategoriesControllerHelper _helper;
        private readonly CancellationToken _cancellationToken;

        public CategoriesControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoriesController(_mockCategoryService.Object);
            _categoryData = new CategoryGenerator();
            _helper = new CategoriesControllerHelper(_mockCategoryService);
            _cancellationToken = CancellationToken.None;
        }

        [Fact]
        public async Task GetCategories_ShouldReturnOkWithCategories()
        {
            // Arrange
            var categories = new List<CategoryDto>
            {
                _categoryData.GenerateFakeDto(),
                _categoryData.GenerateFakeDto(),
                _categoryData.GenerateFakeDto()
            };

            _helper.SetupGetAllAsync(categories);

            // Act
            var result = await _controller.GetCategories(_cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedCategories = okResult.Value
                .Should().BeOfType<List<CategoryDto>>();
        }

        [Fact]
        public async Task GetCategory_ShouldReturnOkWithCategory()
        {
            // Arrange
            var category = _categoryData.GenerateFakeDto();

            _helper.SetupGetByIdAsync(category);

            // Act
            var result = await _controller.GetCategory(category.Id, _cancellationToken);

            // Assert
            var okResult = result
                .Should().BeOfType<OkObjectResult>().Subject;

            var returnedCategory = okResult.Value
                .Should().BeOfType<CategoryDto>();
        }

        [Fact]
        public async Task CreateCategory_ShouldReturnCreatedAtAction()
        {
            // Arrange
            var category = _categoryData.GenerateFakeDto();

            _helper.SetupCreateAsync(category);

            // Act
            var result = await _controller.CreateCategory(category, _cancellationToken);

            // Assert
            var createdAtResult = result
                .Should().BeOfType<CreatedAtActionResult>().Subject;

            var returnedCategory = createdAtResult.Value
                .Should().BeOfType<CategoryDto>();
        }

        [Fact]
        public async Task UpdateCategory_ShouldReturnNoContent()
        {
            // Arrange
            var category = _categoryData.GenerateFakeDto();

            _helper.SetupUpdateAsync(category);

            // Act
            var result = await _controller.UpdateCategory(category, _cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnNoContent()
        {
            // Arrange
            var category = _categoryData.GenerateFakeDto();

            _helper.SetupDeleteAsync(category);

            // Act
            var result = await _controller.DeleteCategory(category.Id, _cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }
    }
}