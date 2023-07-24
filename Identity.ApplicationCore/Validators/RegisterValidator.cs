namespace Identity.ApplicationCore.Validator
{
    public sealed class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator()
        {
            RuleFor(registerDto => registerDto.FirstName)
                .NotEmpty().WithMessage("{PropertyName} must be set")
                .Length(2, 50).WithMessage("Length of {PropertyName} is invalid");

            RuleFor(loginDto => loginDto.LastName)
                .NotEmpty().WithMessage("{PropertyName} must be set")
                .Length(2, 50).WithMessage("Length of {PropertyName} is invalid");

            RuleFor(loginDto => loginDto.Email)
                .NotEmpty().WithMessage("{PropertyName} must be set")
                .EmailAddress();

            RuleFor(loginDto => loginDto.Password!)
                .NotEmpty()
                .WithMessage("{PropertyName} must be set")
                .SetPasswordRules();
        }
    }
}