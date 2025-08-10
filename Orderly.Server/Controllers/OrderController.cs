
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orderly.Server.Data;
using Orderly.Server.Data.Models;
using Orderly.Server.Services;
using Orderly.Shared.Dtos;

namespace Orderly.Server.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly AppDbContext _Context;

        private readonly UserManager<AppUser> _UserManager;

        private readonly OrderService _OrderService;

        public OrderController(AppDbContext context, UserManager<AppUser> userManager, OrderService orderService)
        {
            _Context = context;
            _UserManager = userManager;
            _OrderService = orderService;
        }

        [HttpGet("placed")]
        public async Task<IActionResult> GetUserPlacedOrders()
        {
            // Get the user

            string? userId = _UserManager.GetUserId(User);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Get the placed orders

            var orders = await _Context.Orders
                .Include(o => o.Customer)
                .Include(o => o.Product)
                    .ThenInclude(p => p.Owner)
                .Where(o => o.CustomerId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => o.ToDto())
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("received")]
        public async Task<IActionResult> GetUserReceivedOrders()
        {
            // Get the user

            string? userId = _UserManager.GetUserId(User);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Get the placed orders

            var orders = await _Context.Orders
                .Where(o => o.Product.OwnerId == userId)
                .Include(o => o.Product)
                    .ThenInclude(p => p.Owner)
                .Include(o => o.Customer)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => o.ToDto())
                .ToListAsync();

            return Ok(orders);
        }

        [HttpGet("{orderPublicId}")]
        public async Task<IActionResult> GetOrder(string orderPublicId)
        {
            // Get the user

            string? userId = _UserManager.GetUserId(User);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Get the database order

            Order? order = await _OrderService.GetOrderFromPublicIdAsync(orderPublicId);

            if (order is null)
            {
                return NotFound();
            }

            // The user must be allowed to edit the order (be the seller or customer)

            if (order?.Product?.OwnerId != userId && order?.CustomerId != userId)
            {
                return Forbid();
            }

            // Respond with the order

            return Ok(order.ToDto());
        }

        [HttpPut("{orderPublicId}")]
        public async Task<IActionResult> UpdateOrder(string orderPublicId, [FromBody] OrderDto updatedOrderDto)
        {
            if (orderPublicId != updatedOrderDto.PublicId)
            {
                return BadRequest();
            }

            // Get the user

            string? userId = _UserManager.GetUserId(User);

            if (userId is null)
            {
                return Unauthorized();
            }

            // Get the database order

            Order? order = await _OrderService.GetOrderFromPublicIdAsync(orderPublicId);

            if (order is null)
            {
                return NotFound();
            }

            // The user must be allowed to edit the order (be the seller or customer)

            if (order?.Product?.OwnerId != userId && order?.CustomerId != userId)
            {
                return Forbid();
            }

            // An order cannot be updated if it is complete or cancelled

            if (order.Status == OrderStatus.Complete || order.Status == OrderStatus.Cancelled)
            {
                return Conflict();
            }

            // Parse the status

            OrderStatus status;

            if (!Enum.TryParse(updatedOrderDto.Status, ignoreCase: true, out status))
            {
                return BadRequest();
            }

            // Customers can only set the order status to cancelled

            if (order.CustomerId == userId)
            {
                if (status != OrderStatus.Cancelled)
                {
                    return Forbid();
                }
            }

            // Apply the update

            order.Status = status;

            await _OrderService.UpdateOrderAsync(order);

            return NoContent();
        }
    }
}
