using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Shared.Dtos.LikeDtos;
using API.Shared.Dtos.PostDtos;

namespace API.IService
{
    public interface ILikeService
    {
        Task<string> ToggleLikeAsync(string userId, int postId); 
    }
}
