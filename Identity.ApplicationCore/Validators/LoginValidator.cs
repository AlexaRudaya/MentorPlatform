namespace Identity.ApplicationCore.Validator
{
    public sealed class LoginValidator : AbstractValidator<LoginDto>
    {
        public LoginValidator()
        {
            RuleFor(loginDto => loginDto.Email)
                .NotEmpty()
                .WithMessage("The email must be set")             
                .EmailAddress();

            RuleFor(loginDto => loginDto.Password!)
                .NotEmpty()
                .WithMessage("{PropertyName} must be set")
                .SetPasswordRules();
        }
    }
}