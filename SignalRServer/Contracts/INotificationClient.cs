using SignalRServer.Models;

namespace SignalRServer.Contracts;

public interface INotificationClient
{
    Task ReceiveNotification(string content);
    Task RecieveEmployeeDetails(Employee employee);
    Task NotifyUser(string content, string connectionId);
}
