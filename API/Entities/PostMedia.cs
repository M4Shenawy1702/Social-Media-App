using System;

namespace API.Entities
{
    public class PostMedia
    {
        public int Id { get; set; }
        public string Url { get; set; } = null!;
        public MediaType Type { get; set; } = MediaType.Image;
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
    }

    public enum MediaType
    {
        Image,
        Video
    }
}
