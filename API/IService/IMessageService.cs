using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Dtos;
using API.Shared.Dtos.CommentDtos;
using API.Shared.Dtos.MessageDots;

namespace API.Contracts
{
    public interface IMessageService
    {
        Task<bool> DeleteChatAsync(int chatId);
        Task<MessageDto> SaveMessageAsync(MessageDto message);
        Task<IEnumerable<MessageDetailsDto>> GetMessagesAsync(string user1, string user2);
        Task<MessageDto> UpdateMessageAsync(int messageId, string content, string userId);
        Task<MessageDto> DeleteMessageAsync(int messageId, string userId);
    }

}