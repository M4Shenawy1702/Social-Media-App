using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    public class GetPostByUserIdAndPostId
    : BaseSpecifications<Post>
    {
        public GetPostByUserIdAndPostId(int postId, string userId)
        : base(p => p.Id == postId && p.AuthorId == userId)
        {
        }
    }
}