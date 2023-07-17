namespace Identity.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountServic)
        {
            _accountService = accountServic;
        }

        [HttpPost("Register")]
        [ProducesResponseType(201)]
        [ProducesResponseType(400)]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto,
            CancellationToken cancellationToken = default)
        {
            await _accountService.RegisterAsync(registerDto, cancellationToken);

            return Ok();
        }

        [HttpPost("Login")]
        [ProducesResponseType(204)]
        [ProducesResponseType(400)]

        public async Task<IActionResult> Login([FromBody] LoginDto loginDto, 
            CancellationToken cancellationToken)
        { 
            await _accountService.LoginAsync(loginDto, cancellationToken);

            return NoContent();
        }
    }
}