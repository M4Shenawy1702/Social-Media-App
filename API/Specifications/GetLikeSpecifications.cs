using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    public class GetLikeSpecifications
    : BaseSpecifications<Like>
    {
        public GetLikeSpecifications(string userId, int postId)
        : base(l => l.PostId == postId && l.UserId == userId)
        {
        }
    }
}