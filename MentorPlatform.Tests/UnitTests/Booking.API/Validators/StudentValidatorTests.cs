namespace MentorPlatform.Tests.UnitTests.Booking.API.Validators
{
    public class StudentValidatorTests
    {
        private readonly StudentValidator _studentValidator;
        private readonly StudentGenerator _studentGenerator;

        public StudentValidatorTests()
        {
            _studentValidator = new StudentValidator();
            _studentGenerator = new StudentGenerator();
        }

        [Fact]
        public async Task ValidateStudentCreateDto_WhenModelIsValid_ShouldBeSuccessfulValidation()
        {
            // Arrange
            var studentDto = _studentGenerator.GenerateFakeStudentCreateDto();

            // Act
            var result = await _studentValidator.TestValidateAsync(studentDto);

            // Assert
            result
                .ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("", "Name")]
        [InlineData("A", "Name")]
        [InlineData("This is really too much characters for the Name property. There should be less than this amount.", "Name")]
        [InlineData("", "Email")]
        [InlineData("emailWithoutAddressSign", "Email")]
        public async Task ValidateStudentCreateDto_WhenValuesAreInvalid_ShouldFailValidation(string value, string propertyName)
        {
            // Arrange
            var studentDto = _studentGenerator.GenerateFakeStudentCreateDto();
            typeof(StudentCreateDto).GetProperty(propertyName).SetValue(studentDto, value);

            // Act
            var result = await _studentValidator.TestValidateAsync(studentDto);

            // Assert
            result
                .ShouldHaveValidationErrorFor(propertyName);
        }
    }
}