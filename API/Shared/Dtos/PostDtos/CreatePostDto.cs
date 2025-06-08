using Microsoft.AspNetCore.Http;

namespace API.Shared.Dtos.PostDtos
{
    public class CreatePostDto
    {
        public required string Content { get; set; } = null!;
        public IFormFile? Media { get; set; } 
    }

}