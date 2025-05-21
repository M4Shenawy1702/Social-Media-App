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
        Console.WriteLine("🔥 SendMessage triggered");
        Console.WriteLine($"👤 SenderId: {senderId}");
        Console.WriteLine($"📨 ReceiverId: {userId}");
        Console.WriteLine($"💬 Message: {message}");

        if (string.IsNullOrEmpty(senderId))
            throw new Exception("Context.UserIdentifier is null");

        var msg = new MessageDto
        {
            SenderId = senderId,
            ReceiverId = userId,
            Content = message,
            Timestamp = DateTime.UtcNow
        };

        Console.WriteLine("💾 Saving message to DB");
        await _messageService.SaveMessageAsync(msg);

        Console.WriteLine("📤 Sending to receiver");
        await Clients.User(userId).SendAsync("ReceiveMessage", senderId, message);

        Console.WriteLine("✅ Message sent");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"💥 ERROR in SendMessage: {ex.Message}");
        Console.WriteLine($"💥 StackTrace: {ex.StackTrace}");
        throw; // Re-throw so client sees failure
    }
}


    public override async Task OnConnectedAsync()
    {
        // Optionally, log connections
        await base.OnConnectedAsync();
    }
}
