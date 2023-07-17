namespace Identity.Infrastructure.Factories
{
    public class ApplicationUserClaimsPrincipalFactory : UserClaimsPrincipalFactory<ApplicationUser>
    {
        public ApplicationUserClaimsPrincipalFactory(UserManager<ApplicationUser> userManager,
            IOptions<IdentityOptions> optionsAccessor) : base(userManager, optionsAccessor) 
        {          
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(ApplicationUser applicationUser)
        { 
            var claimsIdentity = await base.GenerateClaimsAsync(applicationUser);

            if (applicationUser.FirstName is not null) 
            {
                claimsIdentity.AddClaim(new Claim(JwtClaimTypes.GivenName, applicationUser.FirstName));
            }

            if (applicationUser.LastName is not null)
            {
                claimsIdentity.AddClaim(new Claim(JwtClaimTypes.FamilyName, applicationUser.LastName));
            }

            return claimsIdentity;
        }
    }
}