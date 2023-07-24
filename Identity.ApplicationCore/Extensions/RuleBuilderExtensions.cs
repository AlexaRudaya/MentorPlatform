namespace Identity.ApplicationCore.Extensions
{
    public static class RuleBuilderExtensions
    {
        public static void SetPasswordRules<T>(this IRuleBuilder<T, string> ruleBuilder)
        {
            ruleBuilder
                .MinimumLength(5)
                .WithMessage("Minimum length of {PropertyName} must be at least 5")
                .Matches("[a-z]")
                .WithMessage("{PropertyName} must contain at least one lowercase letter")
                .Matches("[A-Z]")
                .WithMessage("{PropertyName} must contain at least one uppercase letter")
                .Matches("[0-9]")
                .WithMessage("{PropertyName} must contain at least one digit")
                .Matches("[^a-zA-Z0-9]")
                .WithMessage("{PropertyName} must contain at least one special character");
        }
    }
}