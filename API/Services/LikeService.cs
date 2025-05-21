using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Dtos.LikeDtos;

namespace API.Services
{
    public class LikeService(IUnitOfWork _unitOfWork, IMapper _mapper)
        : ILikeService
    {
        public async Task<LikeDto> AddLikeAsync(string userId, int postId)
        {
            var userRepo = _unitOfWork.GetRepository<ApplicationUser, string>();
            var user = await userRepo.GetAsync(userId) ??
                throw new UserNotFoundException(userId);

            var postRepo = _unitOfWork.GetRepository<Post, int>();
            var post = await postRepo.GetAsync(postId) ??
                throw new PostNotFoundException(postId);

            var likeRepo = _unitOfWork.GetRepository<Like, int>();    
            var existingLike = await likeRepo.GetAsync(new GetLikeSpecifications(userId, postId));
            if (existingLike != null)
                throw new LikeAlreadyExistsException();

            
            var like = new Like
            {
                PostId = post.Id,
                LikedAt = DateTime.UtcNow,
                UserId = userId,
            };
            likeRepo.Add(like);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<LikeDto>(like);
        }

        public async Task<IEnumerable<LikeDto>> GetUserLikesAsync(string userId)
        {
            var userRepo = _unitOfWork.GetRepository<ApplicationUser, string>();
            var user = await userRepo.GetAsync(new GetUserWithLikesSpecifications(userId)) ??
                throw new UserNotFoundException(userId);

            return _mapper.Map<IEnumerable<LikeDto>>(user.Likes);
        }

        public async Task<bool> HasUserLikedAsync(string userId, int postId)
        {
            var likeRepo = _unitOfWork.GetRepository<Like, int>();
            var like = await likeRepo.GetAsync(new GetLikeSpecifications(userId, postId));
            return like != null;
        }

        public async Task<bool> RemoveLikeAsync(string userId, int postId)
        {
            var likeRepo = _unitOfWork.GetRepository<Like, int>();
            var like = await likeRepo.GetAsync(new GetLikeSpecifications(userId, postId)) ??
                throw new LikeNotFoundException();

            likeRepo.Delete(like);
            await _unitOfWork.SaveChangesAsync();

            return true;
        }

    }
}