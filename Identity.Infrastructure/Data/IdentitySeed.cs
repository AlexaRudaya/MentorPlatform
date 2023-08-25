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
                    Scopes = new List<string> { "IdentityServerApi" }
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
                    Name = "IdentityServerApi",
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
                        ClientName = "Microservice",
                        AllowedGrantTypes = GrantTypes.ClientCredentials,
                        AllowedScopes = new List<string> { "IdentityServerApi" },
                        AllowedCorsOrigins = new List<string> { "https://localhost:443" }
                    }.ToEntity(),
                    new Client
                    {
                        ClientId = Guid.NewGuid().ToString(),
                        ClientSecrets = new List<Secret> { new("secret".Sha512()) },
                        ClientName = "Web App",
                        AllowedGrantTypes = GrantTypes.Code,
                        AllowedScopes = new List<string> { "openid", "profile", "email", "IdentityServerApi" },
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