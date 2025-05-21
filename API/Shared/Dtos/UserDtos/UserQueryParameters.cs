using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Dtos.UserDtos
{
    public class UserQueryParameters
    {
        private const int MaxPageSize = 10;
        private const int DefaultPageSize = 5;

        public UserSortOption? SortOption { get; set; }
        public string? SearchByName { get; set; }
         
        public int PageIndex { get; set; } = 1;

        private int _pageSize = DefaultPageSize;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value > 0 && value <MaxPageSize ? value : DefaultPageSize;
        }
    }
}