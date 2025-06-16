using System.Security.Claims;
using API.Shared.Dtos.RelationshipDtos;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserRelationshipsController : ControllerBase
    {
        private readonly IUserRelationshipService _userRelationshipService;

        public UserRelationshipsController(IUserRelationshipService userRelationshipService)
        {
            _userRelationshipService = userRelationshipService;
        }

        // Send Friend Request
        [HttpPost("send-request/{initiatorId}/{receiverId}")]
        public async Task<IActionResult> SendFriendRequest([FromRoute] string initiatorId, string receiverId)
        {
            // Console.WriteLine($"Endpoint reached for {receiverId}"); 
            // var initiatorId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _userRelationshipService.SendFriendRequestAsync(initiatorId!, receiverId);
            return Ok(new { message = "Friend request sent successfully." });
        }

        // Accept Friend Request
        [HttpPost("accept-request/{relationshipId}")]
        public async Task<IActionResult> AcceptFriendRequest(int relationshipId)
        {
            await _userRelationshipService.AcceptFriendRequestAsync(relationshipId);
            return Ok(new { message = "Friend request accepted successfully." });
        }

        // Decline Friend Request
        [HttpPost("decline-request/{relationshipId}")]
        public async Task<IActionResult> DeclineFriendRequest(int relationshipId)
        {
            await _userRelationshipService.DeclineFriendRequestAsync(relationshipId);
            return Ok(new { message = "Friend request declined successfully." });
        }

        // Cancel Friend Request
        [HttpDelete("cancel-request/{relationshipId}")]
        public async Task<IActionResult> CancelFriendRequest(int relationshipId)
        {
            await _userRelationshipService.CancelFriendRequestAsync(relationshipId);
            return Ok(new { message = "Friend request canceled successfully." });
        }

        // Get Friends
        [HttpGet("get-friends/{userId}")]
        public async Task<IActionResult> GetFriends(string userId)
        {
            var friends = await _userRelationshipService.GetFriendsAsync(userId);
            return Ok(friends);
        }

        // Remove Friend
        [HttpDelete("remove-friend/{friendId}")]
        public async Task<IActionResult> RemoveFriend([FromRoute] string friendId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            await _userRelationshipService.RemoveFriendAsync(userId!, friendId);
            return Ok(new { message = "Friend removed successfully." });
        }

        // Get Received Friend Requests
        [HttpGet("get-received-requests/{userId}")]
        public async Task<ActionResult<IEnumerable<FriendRequestDetailsDto>>> GetReceivedRequests(string userId)
        {
            var receivedRequests = await _userRelationshipService.GetReceivedRequestsAsync(userId);
            return Ok(receivedRequests);
        }

        // Get Sent Friend Requests
        [HttpGet("get-sent-requests/{userId}")]
        public async Task<IActionResult> GetSentRequests(string userId)
        {
            var sentRequests = await _userRelationshipService.GetSentRequestsAsync(userId);
            return Ok(sentRequests);
        }
        [HttpGet("get-friend-status/{FriendId}")]
        public async Task<IActionResult> GetFriendRequests(string FriendId)
        {
            var currentUserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(currentUserId))
                return Unauthorized("User ID not found in token.");
            var friendRequests = await _userRelationshipService.GetFriendStatusAsync(currentUserId!, FriendId);
            return Ok(friendRequests);
        }
    }
}
