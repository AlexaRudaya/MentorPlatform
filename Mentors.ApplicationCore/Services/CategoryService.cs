namespace Mentors.ApplicationCore.Services
{
    public sealed class CategoryService : ICategoryService
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<CategoryService> _logger;

        public CategoryService(
            ICategoryRepository categoryRepository,
            IMapper mapper,
            ILogger<CategoryService> logger)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task<IEnumerable<CategoryDto>> GetAllAsync(CancellationToken cancellationToken = default)
        {

            var allCategories = await _categoryRepository.GetAllByAsync(cancellationToken: cancellationToken);

            if (allCategories is null)
            {
                throw new CategoryNotFoundException("No categories were found");
            }

            var categoriesDto = _mapper.Map<IEnumerable<CategoryDto>>(allCategories);

            _logger.LogInformation("Categories are loaded");

            return categoriesDto;
        }

        public async Task<CategoryDto> GetByIdAsync(Guid categoryId,
            CancellationToken cancellationToken = default)
        {
            var category = await _categoryRepository.GetOneByAsync(expression: category => category.Id.Equals(categoryId),
                                                                   cancellationToken: cancellationToken);

            if (category is null)
            {
                throw new CategoryNotFoundException($"Such category with Id: {categoryId} was not found");
            }

            var categoryDto = _mapper.Map<CategoryDto>(category);

            return categoryDto;
        }

        public async Task<CategoryDto> CreateAsync(CategoryDto categoryDto,
            CancellationToken cancellationToken = default)
        {
            var categoryToCreate = _mapper.Map<Category>(categoryDto);

            await _categoryRepository.CreateAsync(categoryToCreate, cancellationToken);

            _logger.LogInformation($"A category with Id:{categoryToCreate.Id} and Name:{categoryToCreate.Name} is created successfully");

            return categoryDto;
        }

        public async Task<CategoryDto> UpdateAsync(Guid categoryId, CategoryDto categoryDto,
            CancellationToken cancellationToken = default)
        {
            var existingCategory = await _categoryRepository.GetOneByAsync(expression: category => category.Id.Equals(categoryId));

            var categoryToUpdate = _mapper.Map<Category>(categoryDto);

            await _categoryRepository.UpdateAsync(categoryToUpdate, cancellationToken);

            _logger.LogInformation($"Data for Category with Id: {existingCategory.Id} has been successfully updated.");

            return categoryDto;
        }

        public async Task<CategoryDto> DeleteAsync(Guid categoryId, CancellationToken cancellationToken = default)
        {
            var categoryToDelete = await _categoryRepository.GetOneByAsync(expression: category => category.Id.Equals(categoryId));

            if (categoryToDelete is null || !categoryToDelete.Id.Equals(categoryId))
            {
                _logger.LogError($"Failed finding category with Id:{categoryId} while deleting entity.");
                throw new CategoryNotFoundException($"Such category with Id: {categoryId} was not found");
            }

            var categoryDeleted = _mapper.Map<CategoryDto>(categoryToDelete);

            await _categoryRepository.DeleteAsync(categoryToDelete, cancellationToken);

            _logger.LogInformation($"Category with Id: {categoryId} is removed");

            return categoryDeleted;
        }
    }
}