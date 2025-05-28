using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    public class GetLikesByPostIdsByUserIdSpecification
    : BaseSpecifications<Like>
    {
        public GetLikesByPostIdsByUserIdSpecification(string currentUserId, List<int> postIds) 
        : base(like => like.UserId == currentUserId && postIds.Contains(like.PostId))
        {
        }
    }
}