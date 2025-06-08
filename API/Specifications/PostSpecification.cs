using System;
using API.Shared.Dtos.PostDtos;

namespace API.Specifications
{
    internal class PostSpecification : BaseSpecifications<Post>
    {
        public PostSpecification(PostQueryParameters parameters)
            : base(u =>
                (
                    string.IsNullOrWhiteSpace(parameters.Search) ||
                    u.Content.ToLower().Trim().Contains(parameters.Search.ToLower().Trim()) ||
                    u.Author.DisplayName.ToLower().Trim().Contains(parameters.Search.ToLower().Trim()) ||
                    u.Author.UserName!.ToLower().Trim().Contains(parameters.Search.ToLower().Trim())
                )
                &&
                (
                    string.IsNullOrWhiteSpace(parameters.UserId) || u.Author.Id == parameters.UserId
                )
            )
        {
            AddInclude(p => p.Author);
            AddInclude(p => p.Comments);
            AddInclude(p => p.Likes);

            ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }
    }
}
