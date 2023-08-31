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
    public class CategoryServiceTests
    {
        private readonly Mock<IMapper> _mockMapper;
        private readonly Mock<ICategoryRepository> _mockCategoryRepository;
        private readonly Mock<ILogger<CategoryService>> _mockLogger;
        private readonly ICategoryService _categoryService;
        private readonly CategoryGenerator _categoryGenerator;

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
        }

        [Fact]
        public async Task GetAllAsync_ShouldReturnListOfCategoryDto()
        {
            // Arrange
            var categories = new List<Category>
            {
                _categoryGenerator.GenerateFakeCategory(),
                _categoryGenerator.GenerateFakeCategory(),
                _categoryGenerator.GenerateFakeCategory(),
            };
            var cancellationToken = CancellationToken.None;

            _mockCategoryRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<Category>, IIncludableQueryable<Category, object>>>(),
                    null, 
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(categories);

            var categoriesDto = categories.Select(category => new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            }).ToList();

            _mockMapper
                .Setup(mapper =>mapper.Map<IEnumerable<CategoryDto>>(categories))
                .Returns(categoriesDto);

            // Act
            var result = await _categoryService.GetAllAsync(cancellationToken);

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
            var cancellationToken = CancellationToken.None;

            _mockCategoryRepository
                .Setup(repository => repository.GetAllByAsync(
                    It.IsAny<Func<IQueryable<Category>, IIncludableQueryable<Category, object>>>(),
                    null,
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((IEnumerable<Category>)null);

            // Act
            var result = async() => await _categoryService.GetAllAsync(cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task GetByIdAsync_ShouldReturnCategoryDto()
        {
            // Arrange
            var category = _categoryGenerator.GenerateFakeCategory();
            var cancellationToken = CancellationToken.None;

            _mockCategoryRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            _mockMapper
                .Setup(mapper => mapper.Map<CategoryDto>(category))
                .Returns(categoryDto);

            // Act
            var result = await _categoryService.GetByIdAsync(category.Id, cancellationToken);

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
            var cancellationToken = CancellationToken.None;

            _mockCategoryRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category)null);

            // Act
            var result = async() => await _categoryService.GetByIdAsync(categoryId, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task CreateAsync_ShouldReturnCreatedCategoryDto()
        {
            // Arrange
            var category = _categoryGenerator.GenerateFakeCategory();
            var cancellationToken = CancellationToken.None;

            var categoryDto = new CategoryDto
            { 
                Id = category.Id,
                Name = category.Name
            };

            _mockMapper
                .Setup(mapper => mapper.Map<Category>(categoryDto))
                .Returns(category);

            _mockCategoryRepository
                .Setup(repository => repository.CreateAsync(category, cancellationToken))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _categoryService.CreateAsync(categoryDto, cancellationToken);

            // Assert
            result
                .Should().NotBeNull();
            result
                .Id.Should().Be(categoryDto.Id);

            _mockCategoryRepository
                .Verify(repository => repository
                    .CreateAsync(category, cancellationToken),
                    Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ShouldReturnUpdatedCategoryDto()
        {
            // Arrange
            var category = _categoryGenerator.GenerateFakeCategory();
            var cancellationToken = CancellationToken.None;

            var categoryDto = new CategoryDto
            { 
                Id= category.Id,
                Name = category.Name
            };

            _mockCategoryRepository
               .Setup(repository => repository.GetOneByAsync(
                   null,
                   It.IsAny<Expression<Func<Category, bool>>>(),
                   It.IsAny<CancellationToken>()))
               .ReturnsAsync(category);

            _mockMapper
                .Setup(mapper => mapper.Map<Category>(categoryDto))
                .Returns(category);

            // Act
            var result = await _categoryService.UpdateAsync(categoryDto, cancellationToken);

            // Assert
            result
                .Should().NotBeNull();

            _mockCategoryRepository
               .Verify(repository => repository
                   .UpdateAsync(category, cancellationToken),
                   Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_WhenCategoryIsNull_ShouldThrowCategoryNotFoundException()
        {
            // Arrange
            var category = _categoryGenerator.GenerateFakeCategory();
            var cancellationToken = CancellationToken.None;

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            _mockCategoryRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category)null);

            // Act
            var result = async() => await _categoryService.UpdateAsync(categoryDto, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<CategoryNotFoundException>();
        }

        [Fact]
        public async Task DeleteAsync_ShouldDeleteCategory()
        {
            // Arrange
            var category = _categoryGenerator.GenerateFakeCategory();
            var cancellationToken = CancellationToken.None;

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            _mockCategoryRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(category);

            _mockMapper
                .Setup(mapper => mapper.Map<CategoryDto>(category))
                .Returns(categoryDto);

            // Act
            var result = await _categoryService.DeleteAsync(category.Id);

            // Assert
            result
                .Should().NotBeNull();

            _mockCategoryRepository
               .Verify(repository => repository
                   .DeleteAsync(category, cancellationToken),
                   Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_WhenCategoryIsNull_ShouldThrowCategoryNotFoundException()
        {
            // Arrange
            var category = _categoryGenerator.GenerateFakeCategory();
            var cancellationToken = CancellationToken.None;

            var categoryDto = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name
            };

            _mockCategoryRepository
                .Setup(repository => repository.GetOneByAsync(
                    null,
                    It.IsAny<Expression<Func<Category, bool>>>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync((Category)null);

            // Act
            var result = async() => await _categoryService.DeleteAsync(category.Id, cancellationToken);

            // Assert
            await result
                .Should().ThrowAsync<CategoryNotFoundException>();
        }
    }
}