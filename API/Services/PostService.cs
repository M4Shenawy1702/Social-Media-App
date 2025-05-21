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
        public async Task<PostDto> CreatePostAsync(CreatePostDto dto)
        {
            var post = new Post
            {
                Content = dto.Content,
                AuthorId = dto.AuthorId,
                LastUpdatedAt = DateTime.UtcNow,
                Media = []
            };

            foreach (var mediaDto in dto.Media)
            {
                var fileName = $"{Guid.NewGuid()}_{mediaDto.Media.FileName}";
                var savePath = Path.Combine("wwwroot", "images", "posts", fileName);
                var publicUrl = $"/images/posts/{fileName}";

                Directory.CreateDirectory(Path.GetDirectoryName(savePath)!);
                using (var stream = new FileStream(savePath, FileMode.CreateNew))
                {

                    await mediaDto.Media.CopyToAsync(stream);
                }

                post.Media.Add(new PostMedia
                {
                    Url = publicUrl,
                    Type = mediaDto.Type.ToLower() == "video" ? MediaType.Video : MediaType.Image
                });
            }
            _unitOfWork.GetRepository<Post, int>().Add(post);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(post);
        }

        public async Task<string> DeletePostAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Post, int>();

            var post = await repo.GetAsync(id) ??
                throw new PostNotFoundException(id);

            repo.Delete(post);
            await _unitOfWork.SaveChangesAsync();
            return "Post deleted successfully";
        }

        public async Task<PaginatedResult<PostDto>> GetAllPostsAsync(PostQueryParameters parameters)
        {
            var specification = new PostSpecification(parameters);
            var repo = _unitOfWork.GetRepository<Post, int>();
            var posts = await repo.GetAllAsync(specification);
            var data = _mapper.Map<IEnumerable<PostDto>>(posts);
            var pageCount = data.Count();
            var totalCount = await repo.CountAsync(new PostsCountSpecifications(parameters));
            return new(parameters.PageIndex, pageCount, totalCount, data);
        }

        public async Task<PostDto> GetPostByIdAsync(int id)
        {
            var repo = _unitOfWork.GetRepository<Post, int>();

            var post = await repo.GetAsync(new GetPostByIdSpesifications(id)) ??
                throw new PostNotFoundException(id);

            return _mapper.Map<PostDto>(post);
        }

        public async Task<PostDto> UpdatePostAsync(int id, UpdatePostDto dto)
        {
            var repo = _unitOfWork.GetRepository<Post, int>();

            var post = await repo.GetAsync(id) ??
                throw new PostNotFoundException(id);

            post.Content = dto.Content;
            post.LastUpdatedAt = DateTime.UtcNow;

            repo.Update(post);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<PostDto>(post);
        }
    }
}