using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Shared.Dtos;

namespace API.Contracts
{
public interface IMessageService
{
    Task SaveMessageAsync(MessageDto message);
    Task<IEnumerable<MessageDetailsDto>> GetMessagesAsync(string user1, string user2);
}

}