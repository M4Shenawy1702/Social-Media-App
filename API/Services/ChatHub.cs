using API.Shared.Dtos;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

public class ChatHub(IMessageService _messageService) : Hub
{
    public async Task SendMessage(string userId, string message)
    {
        var senderId = Context.UserIdentifier;

        try
        {
            if (string.IsNullOrEmpty(senderId))
                throw new Exception("Context.UserIdentifier is null");

            var msg = new MessageDto
            {
                SenderId = senderId,
                ReceiverId = userId,
                Content = message,
                Timestamp = DateTime.UtcNow
            };

            msg = await _messageService.SaveMessageAsync(msg);

            await Clients.User(userId).SendAsync("ReceiveMessage", msg);
            await Clients.User(senderId).SendAsync("ReceiveMessage", msg); 

        }
        catch (Exception ex)
        {
            Console.WriteLine($"💥 ERROR in SendMessage: {ex.Message}");
            Console.WriteLine($"💥 StackTrace: {ex.StackTrace}");
            throw; // Re-throw so client sees failure
        }
    }
    public override async Task OnConnectedAsync() => await base.OnConnectedAsync();
}
