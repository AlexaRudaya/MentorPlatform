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

        public async static Task ValidateLogin(LoginDto loginDto)
        {
            var validator = new LoginValidator();
            var validationResult = await validator.ValidateAsync(loginDto);

            if (!validationResult.IsValid)
            {
                throw new InvalidValueException(validationResult.ToString());
            }
        }
    }
}