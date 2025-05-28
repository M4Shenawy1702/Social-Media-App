using Microsoft.AspNetCore.Mvc;
using API.IService;
using API.Shared.Dtos.LikeDtos;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize] 
    public class LikesController(ILikeService likeService) : ControllerBase
    {
        private readonly ILikeService _likeService = likeService;

        [HttpPost("{postId}")]
        public async Task<ActionResult<string>> ToggleLikeAsync(int postId)
        {
             var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var like = await _likeService.ToggleLikeAsync(userId, postId);
            return Ok(like);
        }

        // [HttpDelete("{postId}")]
        // public async Task<IActionResult> RemoveLike(int postId)
        // {
        //    var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        //     if (string.IsNullOrWhiteSpace(userId))
        //         return Unauthorized("User ID not found in token.");

        //     var result = await _likeService.RemoveLikeAsync(userId, postId);
        //     return result ? NoContent() : NotFound();
        // }

        [HttpGet("user")]
        public async Task<ActionResult<IEnumerable<LikeDto>>> GetUserLikes()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var likes = await _likeService.GetUserLikesAsync(userId);
            return Ok(likes);
        }

        [HttpGet("has-liked/{postId}")]
        public async Task<ActionResult<bool>> HasUserLiked(int postId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userId))
                return Unauthorized("User ID not found in token.");

            var hasLiked = await _likeService.HasUserLikedAsync(userId, postId);
            return Ok(hasLiked);
        }
    }
}
