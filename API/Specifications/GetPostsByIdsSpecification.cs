using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    public class GetPostsByIdsSpecification
    : BaseSpecifications<Post>
    {
        public GetPostsByIdsSpecification(List<int> postIds)
        : base(p => postIds.Contains(p.Id))
        {
            AddInclude(p => p.Author);
            AddInclude(p => p.Media);
            AddInclude(p => p.Comments);
            AddInclude(p => p.Likes);
        }
    }
}