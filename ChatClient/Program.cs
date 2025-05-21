using Microsoft.AspNetCore.SignalR.Client;

Console.Write("Enter your JWT Token: ");
string token = Console.ReadLine();

Console.Write("Enter Receiver User ID: ");
string receiverId = Console.ReadLine();

var connection = new HubConnectionBuilder()
    .WithUrl("http://localhost:5043/chathub", options =>
    {
        options.AccessTokenProvider = () => Task.FromResult(token);
    })
    .WithAutomaticReconnect()
    .Build();

connection.On<string, string>("ReceiveMessage", (senderId, message) =>
{
    Console.WriteLine($"[From {senderId}]: {message}");
});

await connection.StartAsync();
Console.WriteLine("Connected to chat hub.");

while (true)
{
    Console.Write("You: ");
    var msg = Console.ReadLine();
    await connection.InvokeAsync("SendMessage", receiverId, msg);
}
