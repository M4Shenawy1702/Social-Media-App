namespace API.Entities
{
    public class Like
    {
        public int Id { get; set; }
        public string UserId { get; set; } = null!;
        public ApplicationUser User { get; set; } = null!;
        public int PostId { get; set; }
        public Post Post { get; set; } = null!;
        public DateTime LikedAt { get; set; } 
    }
}
