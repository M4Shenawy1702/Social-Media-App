using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using API.Entities;
using API.Shared.Dtos;
using API.Shared.Dtos.UserDtos;

namespace API.IService
{
    public interface IUserService
    {
        // User Profile Operations
        Task<UserDetailsDto> GetUserProfileAsync(string userId);
        Task<PaginatedResult<UserDetailsDto>> GetAllUserAsync(UserQueryParameters parameters,string currentUserId);
        Task<UserDetailsDto> UpdateUserProfileAsync(string userId, UserUpdateDto updateDto);

        // Online Status Management
        // Task UpdateUserActivityAsync(string userId);
        // Task<bool> GetUserOnlineStatusAsync(string userId);
        // Task<IEnumerable<string>> GetOnlineUsersAsync();
        // Task<IEnumerable<UserConnectionInfoDto>> GetUserConnectionsAsync(string userId);

        // Admin Operations
        // Task BanUserAsync(string adminId, string userId, string reason);
        // Task UnbanUserAsync(string adminId, string userId);
    }
}