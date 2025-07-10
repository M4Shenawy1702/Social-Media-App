using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using API.Shared.Dtos;
using API.Shared.Dtos.MessageDots;
using Microsoft.AspNetCore.SignalR;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class ChatController(IMessageService _messageService, IHubContext<ChatHub> _hubContext) : ControllerBase
    {

        [HttpGet("{receiverId}")]
        public async Task<IActionResult> GetMessages(string receiverId)
        {
            var senderId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var messages = await _messageService.GetMessagesAsync(senderId!, receiverId);
            return Ok(messages);
        }
        [HttpPut("{messageId}")]
        public async Task<IActionResult> UpdateMessage(int messageId, [FromBody] UpdateMessageDto messageDto)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(currentUserId))
                return Unauthorized("User ID not found in token.");

            var updatedMessage = await _messageService.UpdateMessageAsync(messageId, messageDto.Content, currentUserId!);

            await _hubContext.Clients.User(updatedMessage.ReceiverId)
                .SendAsync("MessageUpdated", updatedMessage);

            return Ok(new { message = "Message updated successfully." });
        }
        [HttpDelete("{messageId}")]
        public async Task<IActionResult> DeleteMeesage(int messageId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var deletedMessage = await _messageService.DeleteMessageAsync(messageId, userId!);

            if (deletedMessage == null)
                return NotFound("Message not found or not allowed to delete.");

            // ابعت إشعار للمستلم
            await _hubContext.Clients.User(deletedMessage.ReceiverId)
                .SendAsync("MessageDeleted", new { messageId = deletedMessage.Id });

            return Ok(new { message = "Message deleted successfully." });
        }
        [HttpDelete("by-chat/{chatId}")]
        public async Task<IActionResult> DeleteChatAsync(int chatId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            await _messageService.DeleteChatAsync(chatId);

            return Ok(new { message = "Chat deleted successfully." });
        }
    }
}