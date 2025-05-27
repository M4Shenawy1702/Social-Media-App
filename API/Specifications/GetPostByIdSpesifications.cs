using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    public class GetPostByIdSpesifications
    :BaseSpecifications<Post>
    {

        public GetPostByIdSpesifications(int postId)
        : base(p => p.Id == postId)
        {
            AddInclude(p => p.Author);
            AddInclude(p => p.Media);
            AddInclude(p => p.Comments);
            AddInclude(p => p.Likes);
        }
    }
}