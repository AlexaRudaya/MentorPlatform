namespace Mentors.ApplicationCore.Validators
{
    public sealed class MentorshipSubjectValidator : AbstractValidator<MentorshipSubjectDto>
    {
        public MentorshipSubjectValidator()
        {
            RuleFor(subjectDto => subjectDto.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("The name must be set")
                .Length(10, 300);
        }
    }
}