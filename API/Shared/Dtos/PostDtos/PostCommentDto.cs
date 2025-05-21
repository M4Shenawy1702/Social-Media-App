using System;

namespace API.Shared.Dtos.PostDtos
{
    public class PostCommentDto
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public string? AuthorName { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
