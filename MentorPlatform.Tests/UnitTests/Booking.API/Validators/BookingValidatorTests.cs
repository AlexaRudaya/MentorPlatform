namespace MentorPlatform.Tests.UnitTests.Booking.API.Validators
{
    public class BookingValidatorTests
    {
        private readonly BookingValidator _bookingValidator;
        private readonly BookingGenerator _bookingGenerator;

        public BookingValidatorTests()
        {
            _bookingValidator = new BookingValidator();
            _bookingGenerator = new BookingGenerator();
        }

        [Fact]
        public async Task ValidateBookingDto_WhenModelIsValid_ShouldBeSuccessfulValidation()
        {
            // Arrange
            var bookingDto = _bookingGenerator.GenerateFakeBookingDto();

            // Act
            var result = await _bookingValidator.TestValidateAsync(bookingDto);

            // Assert
            result
                .ShouldNotHaveAnyValidationErrors();
        }

        [Theory]
        [InlineData("2023-01-01T10:00:00", "StartTimeBooking")]
        [InlineData("2023-01-01T09:00:00", "EndTimeBooking")]
        public async Task ValidateBookingDto_WhenDateTimeValuesAreInvalid_ShouldFailValidation(DateTime value, string propertyName)
        {
            // Arrange
            var bookingDto = _bookingGenerator.GenerateFakeBookingDto();
            typeof(BookingDto).GetProperty(propertyName).SetValue(bookingDto, value);

            // Act
            var result = await _bookingValidator.TestValidateAsync(bookingDto);

            // Assert
            result
                .ShouldHaveValidationErrorFor(propertyName);
        }

        [Theory]
        [InlineData("", "StudentId")]
        [InlineData("", "MentorId")]
        public async Task ValidateBookingDto_WhenIdValuesAreInvalid_ShouldFailValidation(string value, string propertyName)
        {
            // Arrange
            var bookingDto = _bookingGenerator.GenerateFakeBookingDto();
            var propertyValue = propertyName == "StudentId" ? (object)Guid.Empty : value;
            typeof(BookingDto).GetProperty(propertyName).SetValue(bookingDto, propertyValue);

            // Act
            var result = await _bookingValidator.TestValidateAsync(bookingDto);

            // Assert
            result
                .ShouldHaveValidationErrorFor(propertyName);
        }
    }
}