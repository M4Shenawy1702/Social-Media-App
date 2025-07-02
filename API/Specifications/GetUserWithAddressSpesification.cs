using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    internal class GetUserWithAddressSpesification : BaseSpecifications<ApplicationUser>
    {
        public GetUserWithAddressSpesification(UserQueryParameters parameters, string currentUserId)
       : base(u => 
   (string.IsNullOrWhiteSpace(parameters.SearchByName) || u.DisplayName.ToLower().Trim().Contains(parameters.SearchByName.ToLower().Trim()))
   && u.Id != currentUserId)

        {
            AddInclude(u => u.UserAddress!);

            if (parameters.SortOption is not null)
                ApplySort(parameters);
                
            ApplyPagination(parameters.PageSize, parameters.PageIndex);
        }
        private void ApplySort(UserQueryParameters parameters)
        {
            switch (parameters.SortOption)
            {
                case UserSortOption.DisplayNameAsc:
                    AddOrderBy(user => user.DisplayName);
                    break;
                case UserSortOption.DisplayNameDesc:
                    AddOrderByDesc(user => user.DisplayName);
                    break;
                default:
                    break;
            }

        }
    }
}