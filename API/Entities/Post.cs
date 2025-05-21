using System;
using System.Collections.Generic;

namespace API.Entities
{
    public class Post
    {
        public int Id { get; set; }

        public string Content { get; set; } = null!;

        public DateTime LastUpdatedAt { get; set; } = DateTime.UtcNow;

        public string AuthorId { get; set; } = null!;
        public ApplicationUser Author { get; set; } = null!;

        public ICollection<PostMedia> Media { get; set; } = [];
        public ICollection<PostComment> Comments { get; set; } = [];
        public ICollection<Like> Likes { get; set; } = [];
    }
}
