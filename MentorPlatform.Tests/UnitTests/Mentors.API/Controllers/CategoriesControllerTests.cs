using MentorPlatform.Tests.UnitTests.Mentors.API.BogusData;
using Mentors.API.Controllers;
using Mentors.ApplicationCore.DTO;
using Mentors.ApplicationCore.Interfaces.IService;
using Moq;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Controllers
{
    public class CategoriesControllerTests
    {
        private readonly Mock<ICategoryService> _mockCategoryService;
        private readonly CategoriesController _controller;
        private readonly CategoryGenerator _categoryData;

        public CategoriesControllerTests()
        {
            _mockCategoryService = new Mock<ICategoryService>();
            _controller = new CategoriesController(_mockCategoryService.Object);
            _categoryData = new CategoryGenerator();
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
            var cancellationToken = CancellationToken.None;

            _mockCategoryService
                .Setup(service => service
                .GetAllAsync(It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            // Act
            var result = await _controller.GetCategories(cancellationToken);

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
            var cancellationToken = CancellationToken.None;

            _mockCategoryService
                .Setup(service => service
                .GetByIdAsync(category.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            // Act
            var result = await _controller.GetCategory(category.Id, cancellationToken);

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
            var cancellationToken = CancellationToken.None;

            _mockCategoryService
               .Setup(service => service.CreateAsync(
                   category, It.IsAny<CancellationToken>()))
               .ReturnsAsync(category);

            // Act
            var result = await _controller.CreateCategory(category, cancellationToken);

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
            var cancellationToken = CancellationToken.None;

            _mockCategoryService
                .Setup(service => service.UpdateAsync(
                    category, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            // Act
            var result = await _controller.UpdateCategory(category, cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }

        [Fact]
        public async Task DeleteCategory_ShouldReturnNoContent()
        {
            // Arrange
            var category = _categoryData.GenerateFakeDto();
            var cancellationToken = CancellationToken.None;

            _mockCategoryService
                .Setup(service => service.DeleteAsync(
                    category.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            // Act
            var result = await _controller.DeleteCategory(category.Id, cancellationToken);

            // Assert
            result
                .Should().BeOfType<NoContentResult>();
        }
    }
}