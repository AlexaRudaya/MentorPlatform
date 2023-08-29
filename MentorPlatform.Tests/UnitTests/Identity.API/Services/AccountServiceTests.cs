using Identity.ApplicationCore.Services;
using MentorPlatform.Tests.UnitTests.Identity.API.BogusData;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace MentorPlatform.Tests.UnitTests.Identity.API.Services
{
    public class AccountServiceTests
    {
        private readonly UserManager<ApplicationUser> _mockUserManager;
        private readonly SignInManager<ApplicationUser> _mockSignInManager;
        private readonly IMapper _mockMapper;
        private readonly ILogger<AccountService> _mockLogger;
        private readonly IAccountService _accountService;
        private readonly RegisterDataGenerator _registerData;

        public AccountServiceTests()
        {
            _mockUserManager = Substitute.For<UserManager<ApplicationUser>>(
                Substitute.For<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null);

            _mockSignInManager = Substitute.For<SignInManager<ApplicationUser>>(
                _mockUserManager,
                Substitute.For<IHttpContextAccessor>(),
                Substitute.For<IUserClaimsPrincipalFactory<ApplicationUser>>(),
                null, null, null, null);

            _mockMapper = Substitute.For<IMapper>();
            _mockLogger = Substitute.For<ILogger<AccountService>>();
            _registerData = new RegisterDataGenerator();

            _accountService = new AccountService(
                _mockUserManager,
                _mockSignInManager,
                _mockMapper,
                _mockLogger);
        }

        [Fact]
        public async Task RegisterAsync_ValidInput_ReturnsSuccess()
        {
            // Arrange
            var registerDto = _registerData.GenerateFakeData();
            var cancellationToken = CancellationToken.None;

            var applicationUser = new ApplicationUser();

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

            // Act
            await _accountService.RegisterAsync(registerDto, cancellationToken);

            // Assert
            await _mockUserManager
                .Received(1)
                .CreateAsync(applicationUser, registerDto.Password);
        }
    }
}