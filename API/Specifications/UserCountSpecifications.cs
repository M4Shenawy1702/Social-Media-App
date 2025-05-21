using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    internal class UserCountSpecifications : BaseSpecifications<ApplicationUser>
    {
        public UserCountSpecifications(UserQueryParameters parameters)
        : base(u => string.IsNullOrWhiteSpace(parameters.SearchByName) || u.DisplayName.ToLower().Trim().Contains(parameters.SearchByName.ToLower().Trim()))
        {
        }
    }
}