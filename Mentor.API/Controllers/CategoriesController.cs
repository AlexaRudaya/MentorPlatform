﻿namespace Mentors.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryService _categoryService;

        public CategoriesController(
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategories(CancellationToken cancellationToken = default)
        {
            var categories = await _categoryService.GetAllAsync(cancellationToken);

            return Ok(categories);
        }

        [HttpGet("{id:Guid}")]
        [ProducesResponseType(404)]
        public async Task<IActionResult> GetCategory([FromRoute] Guid id,
                                                     CancellationToken cancellationToken = default)
        {
            var category = await _categoryService.GetByIdAsync(id, cancellationToken);

            return Ok(category);
        }

        [Authorize]
        [HttpPost]
        [ProducesResponseType(201)]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryDto categoryDto,
                                                        CancellationToken cancellationToken = default)
        {
            var categoryToCreate = await _categoryService.CreateAsync(categoryDto, cancellationToken);

            return Ok("Successfully created");
        }

        [Authorize]
        [HttpPatch("{id:Guid}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> UpdateCategory([FromRoute] Guid id,
                                                        [FromBody] CategoryDto categoryDto,
                                                        CancellationToken cancellationToken = default)
        {
            var categoryToUpdate = await _categoryService.UpdateAsync(id, categoryDto, cancellationToken);

            return NoContent();
        }

        [Authorize]
        [HttpDelete("{id:Guid}")]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public async Task<IActionResult> DeleteCategory([FromRoute] Guid id,
                                                        CancellationToken cancellationToken = default)
        {
            var categoryToToDelete = await _categoryService.DeleteAsync(id, cancellationToken);

            return NoContent();
        }
    }
}