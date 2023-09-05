namespace MentorPlatform.Tests.UnitTests.Mentors.API.Validators
{
    public class CategoryValidatorTests
    {
        private readonly CategoryValidator _categoryValidator;
        private readonly CategoryGenerator _categoryData;

        public CategoryValidatorTests() 
        {
            _categoryValidator = new CategoryValidator();
            _categoryData = new CategoryGenerator();
        }

        [Fact]
        public async Task ValidateCategoryDto_ShouldBeSuccessfulValidation()
        {
            // Arrange
            var categoryDto = _categoryData.GenerateFakeDto();

            // Act
            var result = await _categoryValidator.TestValidateAsync(categoryDto);

            // Assert
            result
                .ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("P")]
        public async Task ValidateCategoryDto_InvalidValues_ShouldFailValidation(string invalidName)
        {
            // Arrange
            var categoryDto = _categoryData.GenerateFakeDto();
            categoryDto.Name = invalidName;

            // Act
            var result = await _categoryValidator.TestValidateAsync(categoryDto);

            // Assert
            result
                .ShouldHaveValidationErrorFor(categoryDto => categoryDto.Name);
        }
    }
}