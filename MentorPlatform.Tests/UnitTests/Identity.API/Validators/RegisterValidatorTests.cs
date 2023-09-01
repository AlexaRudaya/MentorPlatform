namespace MentorPlatform.Tests.UnitTests.Identity.API.Validators
{
    public class RegisterValidatorTests
    {
        private readonly RegisterValidator _registerValidator;
        private readonly RegisterDataGenerator _registerData;

        public RegisterValidatorTests()
        {
            _registerValidator = new RegisterValidator();
            _registerData = new RegisterDataGenerator();
        }

        [Fact]
        public async Task ValidateRegisterDto_ShouldBeSuccessfulValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", "FirstName")]
        [InlineData("L", "FirstName")]
        [InlineData("", "LastName")]
        [InlineData("W", "LastName")]
        [InlineData("", "Email")]
        [InlineData("emailWithoutAddressSign", "Email")]
        [InlineData("abc", "Password")]
        [InlineData("aBcdf127", "Password")]
        [InlineData("aBcdfKl!", "Password")]
        [InlineData("abcdfk2l!", "Password")]
        [InlineData("ABCDFG9!", "Password")]
        public async Task ValidateRegisterDto_InvalidValues_ShouldFailValidation(string value, string propertyName)
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            typeof(RegisterDto).GetProperty(propertyName).SetValue(registerDto, value);

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result
                .ShouldHaveValidationErrorFor(propertyName);
        }
    }
}