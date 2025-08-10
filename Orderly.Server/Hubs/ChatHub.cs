
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Orderly.Server.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {

    }
}
