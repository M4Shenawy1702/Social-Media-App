using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Dtos.CommentDtos;
using Microsoft.Identity.Client;

namespace API.Services
{
    public class CommentService(IUnitOfWork _unitOfWork, IMapper _mapper)
    : ICommentService
    {
        public async Task<CommentDto> AddCommentAsync(CreateCommentDto dto)
        {
            var userRepo = _unitOfWork.GetRepository<ApplicationUser, string>();
            var user = await userRepo.GetAsync(dto.AuthorId) ??
                throw new UserNotFoundException(dto.AuthorId);

            var postRepo = _unitOfWork.GetRepository<Post, int>();
            var post = await postRepo.GetAsync(dto.PostId) ??
                throw new PostNotFoundException(dto.PostId);

            var commentRepo = _unitOfWork.GetRepository<PostComment, int>();
            var comment = new PostComment
            {
                Content = dto.Content,
                AuthorName = user.DisplayName,
                PostId = post.Id,
                LastUpdatedAt = DateTime.UtcNow,
            };
            commentRepo.Add(comment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }

        public async Task<string> DeleteCommentAsync(int id)
        {
            var commentRepo = _unitOfWork.GetRepository<PostComment, int>();
            var comment = await commentRepo.GetAsync(id) ??
                throw new CommentNotFoundException(id);

            commentRepo.Delete(comment);
            await _unitOfWork.SaveChangesAsync();

            return "Comment deleted successfully";
        }

        public async Task<IEnumerable<CommentDto>> GetCommentsByPostIdAsync(int postId)
        {
            var postRepo = _unitOfWork.GetRepository<Post, int>();
            var post = await postRepo.GetAsync(postId) ??
                throw new PostNotFoundException(postId);

            var commentRepo = _unitOfWork.GetRepository<PostComment, int>();
            var comment = await commentRepo.GetAllAsync(new GetCommentsByPostIdSepcification(postId));

            return _mapper.Map<IEnumerable<CommentDto>>(comment);
        }

        public async Task<CommentDto> UpdateCommentAsync(int id, UpdateCommentDto dto)
        {
            var userRepo = _unitOfWork.GetRepository<ApplicationUser, string>();
            var user = await userRepo.GetAsync(dto.AuthorId) ??
                throw new UserNotFoundException(dto.AuthorId);

            var postRepo = _unitOfWork.GetRepository<Post, int>();
            var post = await postRepo.GetAsync(dto.PostId) ??
                throw new PostNotFoundException(dto.PostId);

            var commentRepo = _unitOfWork.GetRepository<PostComment, int>();
            var comment = await commentRepo.GetAsync(id) ??
                throw new CommentNotFoundException(id);

            comment.Content = dto.Content;
            comment.LastUpdatedAt = DateTime.UtcNow;

            commentRepo.Update(comment);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<CommentDto>(comment);
        }
    }
}