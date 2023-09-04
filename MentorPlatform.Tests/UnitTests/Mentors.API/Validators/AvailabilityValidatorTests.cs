using MentorPlatform.Tests.UnitTests.Mentors.API.BogusData;
using Mentors.ApplicationCore.DTO;
using Mentors.ApplicationCore.Validators;

namespace MentorPlatform.Tests.UnitTests.Mentors.API.Validators
{
    public class AvailabilityValidatorTests
    {
        private readonly AvailabilityValidator _availabilityValidator;
        private readonly AvailabilityGenerator _availabilityData;

        public AvailabilityValidatorTests()
        {
            _availabilityValidator = new AvailabilityValidator();
            _availabilityData = new AvailabilityGenerator();
        }

        [Fact]
        public async Task ValidateAvailabilityDto_ShouldBeSuccessfulValidation()
        {
            // Arrange
            var availabilityDto = _availabilityData.GenerateFakeDto();

            // Act
            var result = await _availabilityValidator.TestValidateAsync(availabilityDto);

            // Assert
            result
                .ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("2023-02-02", "Date")]
        [InlineData("2023-02-02T12:00:00", "StartTime")]
        [InlineData("2023-02-02T09:00:00", "EndTime")]
        public async Task ValidateAvailabilityDto_InvalidValues_ShouldFailValidation(DateTime value, string propertyName)
        {
            // Arrange
            var availabilityDto = _availabilityData.GenerateFakeDto();
            typeof(AvailabilityDto).GetProperty(propertyName).SetValue(availabilityDto, value);

            // Act
            var result = await _availabilityValidator.TestValidateAsync(availabilityDto);

            // Assert
            result
                .ShouldHaveValidationErrorFor(propertyName);
        }
    }
}