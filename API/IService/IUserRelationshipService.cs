using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Dtos.RelationshipDtos;
using API.Shared.Dtos.UserDtos;

namespace API.IService
{
    public interface IUserRelationshipService
    {
        Task SendFriendRequestAsync(string initiatorId, string receiverId);
        Task AcceptFriendRequestAsync(int relationshipId);
        Task DeclineFriendRequestAsync(int relationshipId);
        Task CancelFriendRequestAsync(int relationshipId);
        Task RemoveFriendAsync(string userId, string friendId);
        Task<List<FriendRequestDetailsDto>> GetFriendsAsync(string userId);
        Task<List<FriendReceivedRequestDetailsDto>> GetReceivedRequestsAsync(string userId);
        Task<List<FriendRequestDetailsDto>> GetSentRequestsAsync(string userId);
        Task<RelationshipStatus> GetFriendStatusAsync(string userId, string friendId);
}

}