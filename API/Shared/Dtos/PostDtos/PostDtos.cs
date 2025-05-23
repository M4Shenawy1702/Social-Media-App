using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Dtos.PostDtos
{
    public class PostDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public string AuthorId { get; set; } = null!;
        public ICollection<PostMediaDetailsDto> Media { get; set; } = [];
        public ICollection<PostCommentDto> Comments { get; set; } = [];
        public int Likes { get; set; }
    }
}