namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController(IUserService _userService) 
    : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserDetailsDto>>> GetAllUsersAsync([FromQuery]UserQueryParameters parameters)
        {
                var users = await _userService.GetAllUserAsync(parameters);
                return Ok(users);
        } 
        [HttpGet("{id}")]
        public async Task<ActionResult<ApplicationUser>> GetUserAsync(string id)
        {
            // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.GetUserProfileAsync(id);
            return Ok(user);
        } 
        [HttpPut("update/{id}")]
        public async Task<ActionResult<ApplicationUser>> UpdateUserAsync([FromForm] UserUpdateDto updateDto,string id)
        {   
            // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = await _userService.UpdateUserProfileAsync(id,updateDto);
            return Ok(user);
        } 
    }
}