using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Dtos.PostDtos;

namespace API.Specifications
{
    internal class PostsCountSpecifications : BaseSpecifications<Post>
    {
        public PostsCountSpecifications(PostQueryParameters parameters)
        : base(u => string.IsNullOrWhiteSpace(parameters.Search) || u.Content.ToLower().Trim().Contains(parameters.Search.ToLower().Trim()))
        {
        }
    }
}