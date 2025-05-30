using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Dtos.LikeDtos;
using API.Shared.Dtos.PostDtos;

namespace API.Services
{
    public class LikeService(IUnitOfWork _unitOfWork, IMapper _mapper)
        : ILikeService
    {
        public async Task<string> ToggleLikeAsync(string userId, int postId)
        {
            var userRepo = _unitOfWork.GetRepository<ApplicationUser, string>();
            var user = await userRepo.GetAsync(userId) ??
                throw new UserNotFoundException(userId);

            var postRepo = _unitOfWork.GetRepository<Post, int>();
            var post = await postRepo.GetAsync(postId) ??
                throw new PostNotFoundException();

            var likeRepo = _unitOfWork.GetRepository<Like, int>();
            var existingLike = await likeRepo.GetAsync(new GetLikeSpecifications(userId, postId));
            if (existingLike != null)
            {
                likeRepo.Delete(existingLike);
                await _unitOfWork.SaveChangesAsync();
                return "Like removed successfully";
            }

            var like = new Like
            {
                PostId = post.Id,
                LikedAt = DateTime.UtcNow,
                UserId = userId,
            };
            likeRepo.Add(like);
            await _unitOfWork.SaveChangesAsync();

            return "Like added successfully";
        }

        

    }
}