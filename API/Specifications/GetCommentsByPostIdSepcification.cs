using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    internal class GetCommentsByPostIdSepcification
    : BaseSpecifications<PostComment>
    {
        public GetCommentsByPostIdSepcification(int postId)
        : base(c => c.PostId == postId)
        {

        }
    }
}