using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Dtos.CommentDtos;

namespace API.Shared.Dtos.PostDtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string AuthorId { get; set; } = null!;
        public string AuthorName { get; set; } = null!;
        public string AuthorImageUrl { get; set; } = null!;
        public string? MediaUrl { get; set; } 
        public ICollection<CommentDto> Comments { get; set; } = [];
        public int Likes { get; set; }
        public bool IsLiked { get; set; }
    }
}