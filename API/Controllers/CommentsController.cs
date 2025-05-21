using API.IService;
using API.Shared.Dtos.CommentDtos;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommentsController(ICommentService _commentService) 
    : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<CommentDto>> AddComment([FromForm] CreateCommentDto dto)
        {
            var comment = await _commentService.AddCommentAsync(dto);
            return CreatedAtAction(nameof(GetCommentsByPostId), new { postId = comment.PostId }, comment);
        }

        [HttpGet("post/{postId}")]
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetCommentsByPostId(int postId)
        {
            var comments = await _commentService.GetCommentsByPostIdAsync(postId);
            return Ok(comments);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CommentDto>> UpdateComment(int id, [FromForm] UpdateCommentDto dto)
        {
            var updatedComment = await _commentService.UpdateCommentAsync(id, dto);
            return Ok(updatedComment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int id)
        {
            var result = await _commentService.DeleteCommentAsync(id);
            return Ok(result);
        }
    }
}
