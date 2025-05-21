using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Shared.Dtos.LikeDtos;

namespace API.IService
{
    public interface ILikeService
    {
        Task<LikeDto> AddLikeAsync(string userId, int postId); 
        Task<bool> RemoveLikeAsync(string userId, int postId);
        Task<IEnumerable<LikeDto>> GetUserLikesAsync(string userId);
        Task<bool> HasUserLikedAsync(string userId, int postId);
    }
}
