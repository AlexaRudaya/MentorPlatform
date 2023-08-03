namespace Booking.ApplicationCore.Validators
{
    public class BookingValidator : AbstractValidator<BookingDto>
    {
        public BookingValidator()
        {
            RuleFor(bookingDto => bookingDto.StartTimeBooking)
               .NotEmpty()
               .GreaterThanOrEqualTo(DateTime.UtcNow)
               .WithMessage("{PropertyName} must be in the future");

            RuleFor(bookingDto => bookingDto.EndTimeBooking)
               .NotEmpty()
               .GreaterThan(bookingDto => bookingDto.StartTimeBooking)
               .WithMessage("{PropertyName} must be greater than start time");

            RuleFor(bookingDto => bookingDto.StudentId)
                .NotEmpty()
                .WithMessage("{PropertyName} must be set");

            RuleFor(bookingDto => bookingDto.MentorId)
                .NotEmpty()
                .WithMessage("{PropertyName} must be set");
        }
    }
}