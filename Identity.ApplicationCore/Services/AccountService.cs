namespace Identity.ApplicationCore.Services
{
    public sealed class AccountService : IAccountService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IMapper _mapper;
        private readonly ILogger<AccountService> _logger;

        public AccountService(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IMapper mapper,
            ILogger<AccountService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _mapper = mapper;
            _logger = logger;
        }

        public async Task RegisterAsync(RegisterDto registerDto,
             CancellationToken cancellationToken = default)
        {
            var applicationUser = _mapper.Map<ApplicationUser>(registerDto);

            var userToRegister = await _userManager.CreateAsync(applicationUser, registerDto.Password!);

            if (userToRegister.Succeeded) 
            {
                _logger.LogInformation($"User {applicationUser.Email} registered successfully.");

                await LoginAsync(new()
                {
                    Email = registerDto.Email,
                    Password = registerDto.Password
                }, cancellationToken);
            }
            else
            {
                _logger.LogError($"Invalid register attempt: User {applicationUser.Email} entered the password that is not unique enough.");
            }
        }

        public async Task LoginAsync(LoginDto loginDto, 
            CancellationToken cancellationToken = default)
        { 
            var userToLogin = await _userManager.FindByEmailAsync(loginDto.Email!);

            if (userToLogin is null)
            {
                _logger.LogError($"User {loginDto!.Email} was not found.");
                throw new UserNotFoundException($"User with such email:{loginDto.Email} was not found");
            }

            var passwordCheck = await _signInManager.CheckPasswordSignInAsync(userToLogin!, loginDto.Password, false);

            if (!passwordCheck.Succeeded) 
            {
                _logger.LogError($"Invalid login attempt: User {userToLogin.Email} password is invalid.");
                throw new InvalidPasswordException("The password is invalid");
            }

            _logger.LogInformation($"User {userToLogin.Email} logged in successfully.");
            await _signInManager.SignInAsync(userToLogin!, true);
        }
    }
}