using System.Collections.Generic;
using System.Threading.Tasks;
using API.Shared.Dtos;
using API.Shared.Dtos.PostDtos;

namespace API.Contracts
{
    public interface IPostService
    {
        Task<PaginatedResult<PostDto>> GetAllPostsAsync(PostQueryParameters parameters);
        Task<PostDto> GetPostByIdAsync(int id);
        Task<PostDto> CreatePostAsync(CreatePostDto dto);
        Task<string> DeletePostAsync(int id);
        Task<PostDto> UpdatePostAsync(int id, UpdatePostDto dto);
    }
}
