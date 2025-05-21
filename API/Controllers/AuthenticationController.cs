namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(
            IAuthenticationService authenticationService,
            ILogger<AuthenticationController> logger)
        {
            _authenticationService = authenticationService;
            _logger = logger;
        }

        [HttpGet("login")]
        public async Task<ActionResult<UserResponse>> LoginAsync([FromQuery] LoginRequest request)
        {
            _logger.LogInformation("Login attempt for {Email}", request.Email);
            var response = await _authenticationService.LoginAsync(request);
            return Ok(response);

        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<UserResponse>> RegisterAsync([FromForm] RegisterRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            _logger.LogInformation("Registration attempt for {Email}", request.Email);
            var response = await _authenticationService.RegisterAsync(request);

            return Ok(response);
        }
        [Authorize]
        [HttpPost("send-verification-code/{userId}")]
        public async Task<IActionResult> SendEmailVerificationCode(string userId)
        {
            await _authenticationService.SendEmailVerificationCodeAsync(userId);
            return Ok(new { message = "Verification code sent successfully." });
        }

        // Confirm Email with Verification Code
        [HttpPost("confirm-verification/{userId}")]
        public async Task<IActionResult> ConfirmEmailWithCode(string userId, [FromBody] string code)
        {
            await _authenticationService.ConfirmEmailWithCodeAsync(userId, code);
            return Ok(new { message = "Email confirmed successfully." });
        }
    }
}