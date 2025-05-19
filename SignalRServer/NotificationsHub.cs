using Microsoft.AspNetCore.SignalR;
using SignalRServer.Contracts;
using SignalRServer.Models;

namespace SignalRServer;

public class NotificationsHub : Hub<INotificationClient>
{
    public static Dictionary<string, string> SubscribedUsers = new();

    public async Task SendNotification(string content)
    {
        var X = Context;
        await Clients.All.ReceiveNotification(content);
    }

    public async Task ReceiveNotification(string content, Employee employee)
    {
        await Clients.All.ReceiveNotification($"{content} and sent by: ");
        await Clients.All.RecieveEmployeeDetails(employee);
    }

    public override Task OnConnectedAsync()
    {
        // var X = Context;
        return base.OnConnectedAsync();
    }

    public async Task Login(string userName, string passWord)
    {
        // Login check

        SubscribedUsers.Add(Context.ConnectionId, userName);

        await Clients.Caller.ReceiveNotification("login successful");
    }

    public async Task NotifyUser(string content, string connectionId)
    {
        await Clients.Client(connectionId).ReceiveNotification(content);
    }


}
