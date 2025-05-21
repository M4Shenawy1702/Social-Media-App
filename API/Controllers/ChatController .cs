using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatController(IMessageService _messageService) : ControllerBase
    {

        [HttpGet("{receiverId}")]
        public async Task<IActionResult> GetMessages(string receiverId)
        {
            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var messages = await _messageService.GetMessagesAsync(senderId!, receiverId);
            return Ok(messages);
        }
    }
}