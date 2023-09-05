namespace MentorPlatform.Tests.UnitTests.Mentors.API.Helpers.Categories
{
    public class CategoriesControllerHelper
    {
        private readonly Mock<ICategoryService> _mockCategoryService;

        public CategoriesControllerHelper(
            Mock<ICategoryService> mockCategoryService)
        {
            _mockCategoryService = mockCategoryService;
        }

        public void SetupGetAllAsync(List<CategoryDto> categories)
        {
            _mockCategoryService
                .Setup(service => service.GetAllAsync(
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);
        }

        public void SetupGetByIdAsync(CategoryDto category)
        {
            _mockCategoryService
                .Setup(service => service.GetByIdAsync(
                    category.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);
        }

        public void SetupCreateAsync(CategoryDto category)
        {
            _mockCategoryService
                .Setup(service => service.CreateAsync(
                    category, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);
        }

        public void SetupUpdateAsync(CategoryDto category)
        {
            _mockCategoryService
                .Setup(service => service.UpdateAsync(
                    category, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);
        }

        public void SetupDeleteAsync(CategoryDto category)
        {
            _mockCategoryService
                .Setup(service => service.DeleteAsync(
                    category.Id, It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);
        }
    }
}