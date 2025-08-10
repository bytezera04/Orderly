
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Orderly.Server.Hubs
{
    [Authorize]
    public class OrderHub : Hub
    {

    }
}
