namespace API.Shared.Dtos.CommentDtos
{
    public class UpdateCommentDto
    {
        public required string Content { get; set; } = null!;
        public required int PostId { get; set; }
        public required string AuthorId { get; set; }
    }
}