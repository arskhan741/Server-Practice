using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SignalRServer;
using SignalRServer.Contracts;

namespace ArsalanApp.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RTCsController : ControllerBase
    {
        private readonly IHubContext<NotificationsHub, INotificationClient> _context;

        public RTCsController(IHubContext<NotificationsHub, INotificationClient> context)
        {
            _context = context;
        }

        [HttpGet("NotifySelectedUserByName")]
        public async Task<IActionResult> NotifySelectedUserByName(string userName, string content)
        {
            // Find the connectionId of the user
            var connectionId = NotificationsHub.SubscribedUsers
                .FirstOrDefault(x => x.Value == userName).Key;

            if (connectionId == null)
            {
                return NotFound($"User {userName} not found.");
            }

            var msg = $"Hello {userName}, this is a notification from the server: {content}";

            await _context.Clients.Client(connectionId).NotifyUser(msg, connectionId);

            return Ok($"Notification sent {userName}.");
        }
    }
}
