using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Dtos;
using API.Shared.Dtos.CommentDtos;
using Microsoft.AspNetCore.Http.HttpResults;

namespace API.Services
{
    public class MessageService(IUnitOfWork _unitOFWork, IMapper _mapper)
    : IMessageService
    {

        public async Task SaveMessageAsync(MessageDto dto)
        {
            var chatRepo = _unitOFWork.GetRepository<Chat, int>();
            var messageRepo = _unitOFWork.GetRepository<Message, int>();

            var chat = await chatRepo.GetAsync(new GetChatSpecification(dto.SenderId, dto.ReceiverId));

            if (chat is null)
            {
                chat = new Chat
                {
                    ReceiverId = dto.ReceiverId,
                    SenderId = dto.SenderId,
                };
                chatRepo.Add(chat);
            }

            var message = new Message
            {
                Chat = chat,
                Content = dto.Content,
                Timestamp = DateTime.UtcNow,
                SenderId = dto.SenderId
            };
            messageRepo.Add(message);

            await _unitOFWork.SaveChangesAsync();
        }

        public async Task<IEnumerable<MessageDetailsDto>> GetMessagesAsync(string user1, string user2)
        {
            var chatRepo = _unitOFWork.GetRepository<Chat, int>();
            var messageRepo = _unitOFWork.GetRepository<Message, int>();
            var chat = await chatRepo.GetAsync(new GetChatSpecification(user1, user2));

            if (chat == null || chat.Messages == null)
                return Enumerable.Empty<MessageDetailsDto>();

            var orderedMessages = chat.Messages.OrderBy(m => m.Timestamp).ToList();
            return _mapper.Map<IEnumerable<MessageDetailsDto>>(orderedMessages);
        }
        public async Task<bool> DeleteMessage(int messageId, string userId)
        {
            var messageRepo = _unitOFWork.GetRepository<Message, int>();
            var message = await messageRepo.GetAsync(new GetMessageSpecification(messageId));
            if (message == null)
                return false;
            if (message.SenderId != userId)
                return false;
            messageRepo.Delete(message);
            await _unitOFWork.SaveChangesAsync();
            return true;
        }
        public async Task<CommentDto> UpdateMessage(int messageId, string content, string userId)
        {
            var messageRepo = _unitOFWork.GetRepository<Message, int>();
            var message = await messageRepo.GetAsync(new GetMessageSpecification(messageId));
            if (message == null || message.SenderId != userId)
                throw new MessageNotFoundException();
            message.Content = content;
            await _unitOFWork.SaveChangesAsync();
            return _mapper.Map<CommentDto>(message);
        }
    }

}