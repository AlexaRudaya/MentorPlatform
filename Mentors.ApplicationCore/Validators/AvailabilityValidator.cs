namespace Mentors.ApplicationCore.Validators
{
    public sealed class AvailabilityValidator : AbstractValidator<AvailabilityDto>
    {
        public AvailabilityValidator()
        {
            RuleFor(availabilityDto => availabilityDto.Date)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.Today)
                .WithMessage("Future availability is required");

            RuleFor(availabilityDto => availabilityDto.StartTime)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateTime.UtcNow)
                .WithMessage("Start time must be in the future");

            RuleFor(availabilityDto => availabilityDto.EndTime)
                .NotEmpty()
                .GreaterThan(availabilityDto => availabilityDto.StartTime)
                .WithMessage("End time must be greater than start time");
        }
    }
}