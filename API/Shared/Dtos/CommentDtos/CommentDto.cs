using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Dtos.CommentDtos
{
    public class CommentDto
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime LastUpdatedAt { get; set; }
        public int PostId { get; set; }
        public string? AuthorName { get; set; }
        public string? AuthorId { get; set; }
        public string? AuthorImageUrl { get; set; }
        
    }
}