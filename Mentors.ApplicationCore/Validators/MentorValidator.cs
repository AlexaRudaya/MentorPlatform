namespace Mentors.ApplicationCore.Validators
{
    public sealed class MentorValidator : AbstractValidator<MentorCreateDto>
    {
        public MentorValidator()
        {
            RuleFor(mentorCreateDto => mentorCreateDto.Name)
                .NotEmpty()
                .WithMessage("The name must be set")
                .Length(2, 70);

            RuleFor(mentorCreateDto => mentorCreateDto.Biography)
                .NotEmpty()
                .WithMessage("Biography must be set")
                .MinimumLength(50);

            RuleFor(mentorCreateDto => mentorCreateDto.HourlyRate)
                .NotEmpty()
                .WithMessage("Hourly rate is required")
                .GreaterThan(0)
                .WithMessage("Hourly rate must be greater than zero");

            RuleFor(mentorCreateDto => mentorCreateDto.MeetingDuration)
                .NotEmpty()
                .GreaterThanOrEqualTo(30)
                .WithMessage("Meeting duration must be greater than 30 minutes");
        }
    }
}