using API.Shared.Dtos.PostDtos;
using API.Shared.Dtos;

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PostsController(IPostService _postService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<PostDto>> CreatePost([FromForm] CreatePostDto dto)
        {
            var post = await _postService.CreatePostAsync(dto);
            return CreatedAtAction(nameof(GetPostById), new { id = post.Id }, post);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedResult<PostDto>>> GetAll([FromQuery] PostQueryParameters parameters)
        {
            var posts = await _postService.GetAllPostsAsync(parameters);
            return Ok(posts);
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<PostDto>> GetPostById(int id)
        {
            var post = await _postService.GetPostByIdAsync(id);
            return Ok(post);
        }

        [HttpPut("{id:int}")]
        public async Task<ActionResult<PostDto>> UpdatePost(int id, [FromBody] UpdatePostDto dto)
        {
            var updatedPost = await _postService.UpdatePostAsync(id, dto);
            return Ok(updatedPost);
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<string>> DeletePost(int id)
        {
            var message = await _postService.DeletePostAsync(id);
            return Ok(new { message });
        }
    }
}
