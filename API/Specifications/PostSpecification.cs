using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Dtos.PostDtos;

namespace API.Specifications
{
    internal class PostSpecification
    : BaseSpecifications<Post>
    {
        public PostSpecification(PostQueryParameters parameters)
        : base(u => string.IsNullOrWhiteSpace(parameters.Search) || u.Content.ToLower().Trim().Contains(parameters.Search.ToLower().Trim()))
        {
            AddInclude(p => p.Media);
            AddInclude(p => p.Comments);
            
            ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }
    }
}