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

        // Testing FirstName
        [Fact]
        public async Task ValidateRegisterDto_When_FirstName_IsEmpty_ShouldFailValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            registerDto.FirstName = "";

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldHaveValidationErrorFor(registerDto => registerDto.FirstName);
        }

        [Fact]
        public async Task ValidateRegisterDto_When_FirstName_IsNotLongEnough_ShouldFailValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            registerDto.FirstName = "L";

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldHaveValidationErrorFor(registerDto => registerDto.FirstName);
        }

        // Testing LastName
        [Fact]
        public async Task ValidateRegisterDto_When_LastName_IsEmpty_ShouldFailValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            registerDto.LastName = "";

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldHaveValidationErrorFor(registerDto => registerDto.LastName);
        }


        [Fact]
        public async Task ValidateRegisterDto_When_LastName_IsNotLongEnough_ShouldFailValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            registerDto.LastName = "W";

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldHaveValidationErrorFor(registerDto => registerDto.LastName);
        }

        // Testing Email
        [Fact]
        public async Task ValidateRegisterDto_When_Email_IsEmpty_ShouldFailValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            registerDto.Email = "";

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldHaveValidationErrorFor(registerDto => registerDto.Email);
        }

        [Fact]
        public async Task ValidateRegisterDto_When_Email_IsInvalid_ShouldFailValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            registerDto.Email = "emailWithoutAddressSign";

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldHaveValidationErrorFor(registerDto => registerDto.Email);
        }

        // Testing Password
        [Fact]
        public async Task ValidateRegisterDto_When_Password_IsEmpty_ShouldFailValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            registerDto.Password = "";

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldHaveValidationErrorFor(registerDto => registerDto.Password);
        }

        [Fact]
        public async Task ValidateRegisterDto_When_Password_IsShort_ShouldFailValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            registerDto.Password = "abc";

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldHaveValidationErrorFor(registerDto => registerDto.Password);
        }

        [Fact]
        public async Task ValidateRegisterDto_When_Password_HasNoSpecialChars_ShouldFailValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            registerDto.Password = "aBcdf127";

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldHaveValidationErrorFor(registerDto => registerDto.Password);
        }

        [Fact]
        public async Task ValidateRegisterDto_When_Password_HasNoNumber_ShouldFailValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            registerDto.Password = "aBcdfKl!";

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldHaveValidationErrorFor(registerDto => registerDto.Password);
        }

        [Fact]
        public async Task ValidateRegisterDto_When_Password_HasNoUppercase_ShouldFailValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            registerDto.Password = "abcdfk2l!";

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldHaveValidationErrorFor(registerDto => registerDto.Password);
        }

        [Fact]
        public async Task ValidateRegisterDto_When_Password_HasNoLowercase_ShouldFailValidation()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            registerDto.Password = "ABCDFG9!";

            // Act
            var result = await _registerValidator.TestValidateAsync(registerDto);

            // Assert
            result.ShouldHaveValidationErrorFor(registerDto => registerDto.Password);
        }
    }
}