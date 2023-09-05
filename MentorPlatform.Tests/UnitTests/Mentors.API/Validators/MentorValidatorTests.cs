namespace MentorPlatform.Tests.UnitTests.Mentors.API.Validators
{
    public class MentorValidatorTests
    {
        private readonly MentorValidator _mentorValidator;
        private readonly MentorGenerator _mentorData;

        public MentorValidatorTests()
        {
            _mentorValidator = new MentorValidator();

            var categoryGenerator = new CategoryGenerator();
            var availabilityGenerator = new AvailabilityGenerator();
            _mentorData = new MentorGenerator(categoryGenerator, availabilityGenerator);
        }

        [Fact]
        public async Task ValidateMentorDto_ShouldBeSuccessfulValidation()
        {
            // Arrange
            var mentorDto = _mentorData.GenerateFakeDto();

            // Act
            var result = await _mentorValidator.TestValidateAsync(mentorDto);

            // Assert
            result
                .ShouldNotHaveAnyValidationErrors();
        }


        [Theory]
        [InlineData("", "Name")]
        [InlineData("L", "Name")]
        [InlineData("This is really too much characters for the Name property. There should be less than this amount.", "Name")]
        [InlineData("", "Biography")]
        [InlineData("Short biography", "Biography")]
        public async Task ValidateMentorCreateDto_InvalidStringValues_ShouldFailValidation(string value, string propertyName)
        {
            // Arrange
            var mentorDto = _mentorData.GenerateFakeDto();
            typeof(MentorCreateDto).GetProperty(propertyName).SetValue(mentorDto, value);

            // Act
            var result = await _mentorValidator.TestValidateAsync(mentorDto);

            // Assert
            result
                .ShouldHaveValidationErrorFor(propertyName);
        }

        [Theory]
        [InlineData(0, "HourlyRate")]
        [InlineData(-1, "HourlyRate")]
        [InlineData(0, "MeetingDuration")]
        [InlineData(10, "MeetingDuration")]
        public async Task ValidateMentorCreateDto_InvalidNumericValues_ShouldFailValidation(int value, string propertyName)
        {
            // Arrange
            var mentorDto = _mentorData.GenerateFakeDto();
            typeof(MentorCreateDto).GetProperty(propertyName).SetValue(mentorDto, value);

            // Act
            var result = await _mentorValidator.TestValidateAsync(mentorDto);

            // Assert
            result
                .ShouldHaveValidationErrorFor(propertyName);
        }
    }
}