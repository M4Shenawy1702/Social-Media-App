using System;

namespace API.Entities
{
    public class PostComment
    {
        public int Id { get; set; }
        public string Content { get; set; } = null!;
        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
    }
}
