using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Shared.Dtos.RelationshipDtos
{
    public class FriendRequestDetailsDto
    {
        public int Id { get; set; }

        public string FriendName { get; set; } = null!;

        public string ProfilePictureUrl { get; set; } = null!;

        public string FriendId { get; set; } = null!;
    }
}