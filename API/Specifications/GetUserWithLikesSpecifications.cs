using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    public class GetUserWithLikesSpecifications
    : BaseSpecifications<ApplicationUser>
    {
        public  GetUserWithLikesSpecifications(string userId)
        : base(u => u.Id == userId)
        {
            AddInclude(u => u.Likes);
        }
    }
}