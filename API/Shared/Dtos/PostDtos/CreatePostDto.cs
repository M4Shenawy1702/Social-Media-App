using Microsoft.AspNetCore.Http;

namespace API.Shared.Dtos.PostDtos
{
    public class CreatePostDto
    {
        public required string Content { get; set; } = null!;
        public required string AuthorId { get; set; } = null!;
        public required ICollection<IFormFile> Media { get; set; } = new List<IFormFile>();
    }

}