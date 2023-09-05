namespace MentorPlatform.Tests.UnitTests.Mentors.API.Helpers.Categories
{
    public class CategoryServiceHelper
    {
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<IMapper> _mockMapper;

        public CategoryServiceHelper(
            Mock<ICategoryRepository> mockCategoryRepository,
            Mock<IMapper> mockMapper)
        {
            _mockCategoryRepository = mockCategoryRepository;
            _mockMapper = mockMapper;
        }

        public void SetupGetAllAsync(List<Category> categories)
        {
            _mockCategoryRepository
                .Setup(repository => repository.GetAllByAsync(
                     It.IsAny<Func<IQueryable<Category>, IIncludableQueryable<Category, object>>>(),
                     null,
                     It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);
        }

        public void SetupGetAllAsyncWhenNull()
        {
            _mockCategoryRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<Category>, IIncludableQueryable<Category, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<Category>)null);
        }

        public void SetupGetByIdAsync(Category category)
        {
            _mockCategoryRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);
        }

        public void SetupGetByIdAsyncWhenNull()
        {
            _mockCategoryRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category)null);
        }

        public void SetupCreateAsync(Category category)
        {
            _mockCategoryRepository
                .Setup(repository => repository.CreateAsync(
                    category, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        }

        public void SetupDeleteAsync(Category category)
        {
            _mockCategoryRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);
        }

        public List<CategoryDto> GenerateDtoList(IEnumerable<Category> categories)
        {
            return categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();
        }

        public CategoryDto GenerateDtoFromCategory(Category category)
        {
            return new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };
        }

        public void SetupMapperForCategoryToDto(Category category,
            CategoryDto categoryDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<CategoryDto>(category))
                .Returns(categoryDto);
        }

        public void SetupMapperForDtoToCategory(Category category,
            CategoryDto categoryDto)
        {
            _mockMapper
                .Setup(mapper => mapper.Map<Category>(categoryDto))
                .Returns(category);
        }
    }
}
