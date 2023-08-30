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

        // Testing Email
        [Fact]
        public async Task ValidateLoginDto_When_Email_IsEmpty_ShouldFailValidation()
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            loginDto.Email = "";

            // Act
            var result = await _loginValidator.TestValidateAsync(loginDto);

            // Assert
            result.ShouldHaveValidationErrorFor(loginDto => loginDto.Email);
        }

        [Fact]
        public async Task ValidateLoginDto_When_Email_IsInvalid_ShouldFailValidation()
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            loginDto.Email = "emailWithoutAddressSign";

            // Act
            var result = await _loginValidator.TestValidateAsync(loginDto);

            // Assert
            result.ShouldHaveValidationErrorFor(loginDto => loginDto.Email);
        }

        // Testing Password
        [Fact]
        public async Task ValidateLoginDto_When_Password_IsEmpty_ShouldFailValidation()
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            loginDto.Password = "";

            // Act
            var result = await _loginValidator.TestValidateAsync(loginDto);

            // Assert
            result.ShouldHaveValidationErrorFor(loginDto => loginDto.Password);
        }

        [Fact]
        public async Task ValidateLoginDto_When_Password_IsShort_ShouldFailValidation()
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            loginDto.Password = "abc";

            // Act
            var result = await _loginValidator.TestValidateAsync(loginDto);

            // Assert
            result.ShouldHaveValidationErrorFor(loginDto => loginDto.Password);
        }

        [Fact]
        public async Task ValidateLoginDto_When_Password_HasNoSpecialChars_ShouldFailValidation()
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            loginDto.Password = "aBcdf127";

            // Act
            var result = await _loginValidator.TestValidateAsync(loginDto);

            // Assert
            result.ShouldHaveValidationErrorFor(loginDto => loginDto.Password);
        }

        [Fact]
        public async Task ValidateLoginDto_When_Password_HasNoNumber_ShouldFailValidation()
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            loginDto.Password = "aBcdfKl!";

            // Act
            var result = await _loginValidator.TestValidateAsync(loginDto);

            // Assert
            result.ShouldHaveValidationErrorFor(loginDto => loginDto.Password);
        }

        [Fact]
        public async Task ValidateLoginDto_When_Password_HasNoUppercase_ShouldFailValidation()
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            loginDto.Password = "abcdfk2l!";

            // Act
            var result = await _loginValidator.TestValidateAsync(loginDto);

            // Assert
            result.ShouldHaveValidationErrorFor(loginDto => loginDto.Password);
        }

        [Fact]
        public async Task ValidateLoginDto_When_Password_HasNoLowercase_ShouldFailValidation()
        {
            // Arrange
            var loginDto = _loginData.GenerateFakeData();
            loginDto.Password = "ABCDFG9!";

            // Act
            var result = await _loginValidator.TestValidateAsync(loginDto);

            // Assert
            result.ShouldHaveValidationErrorFor(loginDto => loginDto.Password);
        }
    }
}