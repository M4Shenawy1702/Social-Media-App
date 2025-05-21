using API.Shared.Dtos.CommentDtos;

namespace API.IService
{
    public interface ICommentService
    {
        Task<CommentDto> AddCommentAsync(CreateCommentDto dto);
        Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDto dto);
        Task<string> DeleteCommentAsync(int id);
        Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(int postId);
    }
}
