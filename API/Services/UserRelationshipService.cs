using API.Shared.Dtos.RelationshipDtos;
using Microsoft.AspNetCore.Identity;

namespace API.Services
{
    public class UserRelationshipService(IUnitOfWork _unitOfWork, IMapper _mapper ,UserManager<ApplicationUser> _userManager) 
    : IUserRelationshipService
    {
        public async Task SendFriendRequestAsync(string initiatorId, string receiverId)
        {
            if (initiatorId == receiverId)
                throw new ServiceException(400, "You cannot send a request to yourself.");

            var initiator = await _userManager.FindByIdAsync(initiatorId)??
                throw new UserNotFoundException(initiatorId);
            var receiver = await _userManager.FindByIdAsync(receiverId)??
                throw new UserNotFoundException(receiverId);

            var repo = _unitOfWork.GetRepository<UserRelationship, int>();

            var existing = await repo.GetAsync(new UserRelationshipWithIncludesSpecification(initiatorId, receiverId));

            if (existing != null)
                throw new ServiceException(400, "Friend request already exists or relationship in progress.");

            var request = new UserRelationship
            {
                CreatedAt = DateTime.UtcNow,
                InitiatorId = initiatorId,
                ReceiverId = receiverId,
                Status = RelationshipStatus.Pending,
            };

            repo.Add(request);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AcceptFriendRequestAsync(int relationshipId)
        {
            var repo = _unitOfWork.GetRepository<UserRelationship, int>();

            var relationship = await repo.GetAsync(relationshipId)
                ?? throw new RelationshipNotFoundException(relationshipId);

            if (relationship.Status != RelationshipStatus.Pending)
                throw new ServiceException(400, "Cannot accept non-pending request. The status is either already accepted or declined.");

            relationship.Status = RelationshipStatus.Accepted;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CancelFriendRequestAsync(int relationshipId)
        {
            var repo = _unitOfWork.GetRepository<UserRelationship, int>();

            var relationship = await repo.GetAsync(relationshipId)
                ?? throw new RelationshipNotFoundException(relationshipId);

            if (relationship.Status != RelationshipStatus.Pending)
                throw new ServiceException(400, "Only pending requests can be canceled.");

            repo.Delete(relationship);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeclineFriendRequestAsync(int relationshipId)
        {
            var repo = _unitOfWork.GetRepository<UserRelationship, int>();

            var relationship = await repo.GetAsync(relationshipId)
                ?? throw new RelationshipNotFoundException(relationshipId);

            if (relationship.Status != RelationshipStatus.Pending)
                throw new ServiceException(400, "Only pending requests can be declined.");

            relationship.Status = RelationshipStatus.Declined;
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<FreindRequestDetailsDto>> GetFriendsAsync(string userId)
        {
            var repo = _unitOfWork.GetRepository<UserRelationship, int>();
            var relationships = await repo.GetAllAsync(new UserRelationshipWithIncludesSpecification(userId));

            var friendDtos = relationships
                .Select(r =>
                {
                    var isUserInitiator = r.InitiatorId == userId;
                    var friend = isUserInitiator ? r.Receiver : r.Initiator;

                    return new FreindRequestDetailsDto
                    {
                        Id = r.Id,
                        FriendName = friend.DisplayName,
                        ProfilePictureUrl = friend.ProfilePictureUrl ?? string.Empty,
                        Status = r.Status,
                        CreatedAt = r.CreatedAt
                    };
                })
                .ToList();

            return friendDtos;
        }

        public async Task RemoveFriendAsync(string userId, string friendId)
        {
            var repo = _unitOfWork.GetRepository<UserRelationship, int>();
            var relationship = await repo.GetAsync(new UserRelationshipWithIncludesSpecification(userId, friendId));

            if (relationship == null || relationship.Status != RelationshipStatus.Accepted)
                throw new ServiceException(404, "Friendship not found or already removed.");

            repo.Delete(relationship);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<List<FreindRequestDetailsDto>> GetReceivedRequestsAsync(string userId)
        {
            var repo = _unitOfWork.GetRepository<UserRelationship, int>();
            var requests = await repo.GetAllAsync(new ReceivedFriendRequestsSpecification(userId));
            return _mapper.Map<List<FreindRequestDetailsDto>>(requests);
        }

        public async Task<List<UserDetailsDto>> GetSentRequestsAsync(string userId)
        {
            var repo = _unitOfWork.GetRepository<UserRelationship, int>();
            var requests = await repo.GetAllAsync(new SentFriendRequestsSpecification(userId));
            var receivers = requests.Select(r => r.Receiver).ToList();
            return _mapper.Map<List<UserDetailsDto>>(receivers);
        }
    }
}
