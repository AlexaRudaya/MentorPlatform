namespace Booking.ApplicationCore.Validators
{
    public class StudentValidator : AbstractValidator<StudentCreateDto>
    {
        public StudentValidator()
        {
            RuleFor(studentCreateDto => studentCreateDto.Name)
                .NotEmpty()
                .WithMessage("{PropertyName} must be set")
                .Length(2, 70);

            RuleFor(studentCreateDto => studentCreateDto.Email)
                .NotEmpty().WithMessage("{PropertyName} must be set")
                .EmailAddress();
        }
    }
}