using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Specifications
{
    internal class GetUserWithAddressSpesification : BaseSpecifications<ApplicationUser>
    {
        public GetUserWithAddressSpesification(UserQueryParameters parameters)
        : base(u => string.IsNullOrWhiteSpace(parameters.SearchByName) || u.DisplayName.ToLower().Trim().Contains(parameters.SearchByName.ToLower().Trim()))
        {
            AddInclude(u => u.UserAddress!);

            ApplyPagination(parameters.PageSize, parameters.PageIndex);

            if (parameters.SortOption is not null)
                ApplySort(parameters);
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