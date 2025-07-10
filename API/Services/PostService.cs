using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Dtos;
using API.Shared.Dtos.PostDtos;
using Microsoft.Identity.Client;

namespace API.Services
{
    internal class PostService(IUnitOfWork _unitOfWork, IMapper _mapper)
        : IPostService
    {
        public async Task<PostDto> CreatePostAsync(CreatePostDto dto, string currentUserId)
        {
            string? publicUrl = null;

            if (dto.Media is not null)
            {
                var fileName = $"{Guid.NewGuid()}_{dto.Media.FileName}";
                var savePath = Path.Combine("wwwroot", "images", "posts", fileName);
                publicUrl = $"/images/posts/{fileName}";

                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);

                using (var stream = new FileStream(savePath, FileMode.CreateNew))
                {
                    await dto.Media.CopyToAsync(stream);
                }
            }

            var post = new Post
            {
                Content = dto.Content,
                AuthorId = currentUserId,
                LastUpdatedAt = DateTime.UtcNow,
                MediaUrl = publicUrl!,
            };

            _unitOfWork.GetRepository<Post, int>().Add(post);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(post);
        }


        public async Task<string> DeletePostAsync(int id, string currentUserId)
        {
            var repo = _unitOfWork.GetRepository<Post, int>();

            var post = await repo.GetAsync(new GetPostByUserIdAndPostId(id, currentUserId)) ??
                throw new PostNotFoundException();

            repo.Delete(post);
            await _unitOfWork.SaveChangesAsync();
            return "Post deleted successfully";
        }

        public async Task<PaginatedResult<PostDto>> GetAllPostsAsync(PostQueryParameters parameters, string currentUserId)
        {
            var specification = new PostSpecification(parameters);
            var repo = _unitOfWork.GetRepository<Post, int>();
            var posts = await repo.GetAllAsync(specification);

            var postIds = posts.Select(p => p.Id).ToList();
            var likesRepo = _unitOfWork.GetRepository<Like, int>();

            var likedPostIds = await likesRepo.GetAllAsync(new GetLikesByPostIdsByUserIdSpecification(currentUserId, postIds));
            var likedPostIdSet = likedPostIds.Select(l => l.PostId).ToHashSet();

            var orderedPosts = posts.OrderByDescending(p => p.LastUpdatedAt).ToList();

            var data = _mapper.Map<IEnumerable<PostDto>>(orderedPosts);

            foreach (var postDto in data)
            {
                postDto.IsLiked = likedPostIdSet.Contains(postDto.Id);
            }

            var totalCount = await repo.CountAsync(new PostsCountSpecifications(parameters));

            return new PaginatedResult<PostDto>(parameters.PageIndex, parameters.PageSize, totalCount, data);
        }

        public async Task<List<LIkedPostDto>> GetLikedPostsAsync(string currentUserId)
        {
            var userRepo = _unitOfWork.GetRepository<ApplicationUser, string>();
            var user = await userRepo.GetAsync(new GetUserWithLikesSpecifications(currentUserId))
                ?? throw new UserNotFoundException(currentUserId);

            var likedPostIds = user.Likes.Select(like => like.PostId).ToList();

            var repo = _unitOfWork.GetRepository<Post, int>();
            var posts = await repo.GetAllAsync(new GetPostsByIdsSpecification(likedPostIds));

            var likedPostIdSet = _mapper.Map<List<LIkedPostDto>>(posts);

            return likedPostIdSet;
        }


        public async Task<PostDto> GetPostByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Post, int>();

            var post = await repo.GetAsync(new GetPostByIdSpesifications(id)) ??
                throw new PostNotFoundException();

            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> UpdatePostAsync(int id, UpdatePostDto dto)
        {
            var repo = _unitOfWork.GetRepository<Post, int>();

            var post = await repo.GetAsync(id) ??
                throw new PostNotFoundException();

            post.Content = dto.Content;
            post.LastUpdatedAt = DateTime.UtcNow;

            repo.Update(post);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(post);
        }
    }
}