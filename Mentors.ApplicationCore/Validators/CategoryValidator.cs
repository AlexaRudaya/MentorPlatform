namespace Mentors.ApplicationCore.Validators
{
    public sealed class CategoryValidator : AbstractValidator<CategoryDto>
    {
        public CategoryValidator()
        {
            RuleFor(categoryDto => categoryDto.Name)
                .NotNull()
                .NotEmpty()
                .WithMessage("The name must be set")
                .Length(2, 100);
        }
    }
}