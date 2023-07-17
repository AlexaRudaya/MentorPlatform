namespace Identity.Infrastructure.Data
{
    public sealed class IdentitySeed
    {
        public static async Task SeedAsync(WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            await scope.ServiceProvider.GetRequiredService<ApplicationDbContext>().Database.MigrateAsync();
            await scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>().Database.MigrateAsync();
            await scope.ServiceProvider.GetRequiredService<PersistedGrantDbContext>().Database.MigrateAsync();

            var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            var configurationDbContext = scope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();

            await GetGetPreConfiguredApplicationUsersAsync(userManager);
            await GetPreConfiguredApiResourcesAsync(configurationDbContext);
            await GetPreConfiguredApiScopesAsync(configurationDbContext);
            await GetPreConfiguredClientsAsync(configurationDbContext);
            await GetPreConfiguredIdentityResourcesAsync(configurationDbContext);
        }

        private static async Task GetGetPreConfiguredApplicationUsersAsync(UserManager<ApplicationUser> userManager)
        {

            if (await userManager.FindByNameAsync("John.Smith") is null)
            {
                await userManager.CreateAsync(
                    new ApplicationUser
                    {
                        UserName = "John.Smith",
                        Email = "john.smith@gmail.com",
                        FirstName = "John",
                        LastName = "Smith"
                    }, "pAssword!333");
            }
        }

        private static async Task GetPreConfiguredApiResourcesAsync(ConfigurationDbContext configurationDbContext)
        {
            if (!await configurationDbContext.ApiResources.AnyAsync())
            {
                await configurationDbContext.ApiResources.AddAsync(new ApiResource
                {
                    Name = Guid.NewGuid().ToString(),
                    DisplayName = "API",
                    Scopes = new List<string> { "https://www.example.com/api" }
                }.ToEntity());

                await configurationDbContext.SaveChangesAsync();
            }
        }

        private static async Task GetPreConfiguredApiScopesAsync(ConfigurationDbContext configurationDbContext)
        {
            if (!await configurationDbContext.ApiScopes.AnyAsync())
            {
                await configurationDbContext.ApiScopes.AddAsync(new ApiScope
                {
                    Name = "https://www.example.com/api",
                    DisplayName = "API"
                }.ToEntity());

                await configurationDbContext.SaveChangesAsync();
            }
        }

        private static async Task GetPreConfiguredClientsAsync(ConfigurationDbContext configurationDbContext)
        {
            if (!await configurationDbContext.Clients.AnyAsync())
            {
                await configurationDbContext.Clients.AddRangeAsync(
                    new Client
                    {
                        ClientId = Guid.NewGuid().ToString(),
                        ClientSecrets = new List<Secret> { new("secret".Sha512()) },
                        ClientName = "Console App",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        AllowedScopes = new List<string> { "https://www/example.com/api" },
                        AllowedCorsOrigins = new List<string> { "https://localhost:7001" }
                    }.ToEntity(),
                    new Client
                    {
                        ClientId = Guid.NewGuid().ToString(),
                        ClientSecrets = new List<Secret> { new("secret".Sha512()) },
                        ClientName = "Web App",
                        AllowedGrantTypes = GrantTypes.Code,
                        AllowedScopes = new List<string> { "openid", "profile", "email", "https://www/example.com/api" },
                        RedirectUris = new List<string> { "https://webapplication:7002/signin-oidc" },
                        PostLogoutRedirectUris = new List<string> { "https://webapplication:7002/signout-callback-oidc" }
                    }.ToEntity());

                await configurationDbContext.SaveChangesAsync();
            }
        }

        private static async Task GetPreConfiguredIdentityResourcesAsync(ConfigurationDbContext configurationDbContext)
        {
            if (!await configurationDbContext.IdentityResources.AnyAsync())
            {
                await configurationDbContext.IdentityResources.AddRangeAsync(
                    new IdentityResources.OpenId().ToEntity(),
                    new IdentityResources.Profile().ToEntity(),
                    new IdentityResources.Email().ToEntity());

                await configurationDbContext.SaveChangesAsync();
            }
        }
    }
}