namespace API.Entities
{
    public class Message
    {
        public int Id { get; set; }
        public Chat Chat { get; set; } = null!;
        public int ChatId { get; set; }
        public string Content { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public string SenderId { get; set; }  = null!;
    }
}