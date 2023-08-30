using SignInResult = Microsoft.AspNetCore.Identity.SignInResult;

namespace MentorPlatform.Tests.UnitTests.Identity.API.Helpers
{
    public class AccountServiceTestsHelper
    {
        private readonly UserManager<ApplicationUser> _mockUserManager;
        private readonly SignInManager<ApplicationUser> _mockSignInManager;
        private readonly IMapper _mockMapper;

        public AccountServiceTestsHelper(
            UserManager<ApplicationUser> mockUserManager,
            SignInManager<ApplicationUser> mockSignInManager,
            IMapper mockMapper)
        {
            _mockUserManager = mockUserManager;
            _mockSignInManager = mockSignInManager;
            _mockMapper = mockMapper;
        }

        public void SetUpValidUserForRegister(ApplicationUser applicationUser, 
            RegisterDto registerDto)
        {
            _mockMapper
                .Map<ApplicationUser>(registerDto)
                .Returns(applicationUser);

            _mockUserManager
               .CreateAsync(Arg.Any<ApplicationUser>(), Arg.Any<string>())
               .Returns(IdentityResult.Success);

            _mockUserManager
                .FindByEmailAsync(registerDto.Email)
                .Returns(applicationUser);
            _mockSignInManager.CheckPasswordSignInAsync(applicationUser, registerDto.Password, Arg.Any<bool>())
                .Returns(SignInResult.Success);

        }

        public ApplicationUser SetUpValidUserForLogin(LoginDto loginDto)
        {
            var applicationUser = new ApplicationUser { Email = loginDto.Email };

            _mockUserManager
                .FindByEmailAsync(loginDto.Email)
                .Returns(applicationUser);

            return applicationUser; 
        }
    }
}