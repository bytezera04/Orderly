
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;

namespace Orderly.Server.Hubs
{
    [Authorize]
    public class ProductHub : Hub
    {
    }
}
