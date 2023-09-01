namespace MentorPlatform.Tests.UnitTests.Identity.API.Validators
{
    public class LoginValidatorTests
    {
        private readonly LoginValidator _loginValidator;
        private readonly LoginDataGenerator _loginData;

        public LoginValidatorTests()
        {
            _loginValidator = new LoginValidator();
            _loginData = new LoginDataGenerator();
        }

        [Fact]
        public async Task ValidateLoginDto_ShouldBeSuccessfulValidation()
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();

            // Act
            var result = await _loginValidator.TestValidateAsync(loginDto);

            // Assert
            result.ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", "Email")]
        [InlineData("emailWithoutAddressSign", "Email")]
        [InlineData("abc", "Password")]
        [InlineData("aBcdf127", "Password")]
        [InlineData("aBcdfKl!", "Password")]
        [InlineData("abcdfk2l!", "Password")]
        [InlineData("ABCDFG9!", "Password")]
        public async Task ValidateLoginDto_InvalidValues_ShouldFailValidation(string value, string propertyName)
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            typeof(LoginDto).GetProperty(propertyName).SetValue(loginDto, value);

            // Act
            var result = await _loginValidator.TestValidateAsync(loginDto);

            // Assert
            result
                .ShouldHaveValidationErrorFor(propertyName);
        }
    }
}