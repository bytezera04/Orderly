using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Orderly.Server.Data;
using Orderly.Server.Data.Models;
using Orderly.Server.Hubs;
using Orderly.Shared.Dtos;

namespace Orderly.Server.Services
{
    public class OrderService
    {
        private readonly AppDbContext _Context;

        private readonly IHubContext<OrderHub> _HubContext;

        public OrderService(AppDbContext context, IHubContext<OrderHub> hubContext)
        {
            _Context = context;
            _HubContext = hubContext;
        }

        public async Task AddOrderAsync(Order order)
        {
            // Add this order

            _Context.Orders.Add(order);

            await _Context.SaveChangesAsync();

            // Broadcast event to seller

            if (order.Product?.OwnerId is not null)
            {
                await _HubContext.Clients.User(order.Product.OwnerId)
                    .SendAsync("OrderCreated", order.ToDto());
            }

            // Broadcast event to customer

            if (order.CustomerId is not null)
            {
                await _HubContext.Clients.User(order.CustomerId)
                    .SendAsync("OrderCreated", order.ToDto());
            }
        }

        public async Task UpdateOrderAsync(Order updatedOrder)
        {
            // Try to find this order

            Order? order = await _Context.Orders.FindAsync(updatedOrder.Id);

            if (order is null)
            {
                throw new Exception("Attempted to update an order that does not exist");
            }

            // Update the fields that support updating and save changes

            order.Status = updatedOrder.Status;
            //order.IsDeleted = updatedOrder.IsDeleted;
            // * currently, orders don't support deleting they just get cancelled

            await _Context.SaveChangesAsync();

            // Broadcast event to seller

            if (order.Product?.OwnerId is not null)
            {
                await _HubContext.Clients.User(order.Product.OwnerId)
                    .SendAsync("OrderUpdated", order.ToDto());
            }

            // Broadcast event to customer

            if (order.CustomerId is not null)
            {
                await _HubContext.Clients.User(order.CustomerId)
                    .SendAsync("OrderUpdated", order.ToDto());
            }
        }

        public async Task<List<Order>> GetPlacedOrdersAsync(string userId)
        {
            // Orders placed are orders where the customer is the user

            return await _Context.Orders
                .Include(o => o.Product)
                    .ThenInclude(p => p.Owner)
                .Include(o => o.Customer)
                .Where(o => o.Product != null && o.CustomerId == userId && !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<List<Order>> GetReceivedOrdersAsync(string userId)
        {
            // Orders received are orders where the product belongs to the user

            return await _Context.Orders
                .Include(o => o.Product)
                    .ThenInclude(p => p.Owner)
                .Include(o => o.Customer)
                .Where(o => o.Product != null && o.Product.OwnerId == userId && !o.IsDeleted)
                .ToListAsync();
        }

        public async Task<Order?> GetOrderFromPublicIdAsync(string publicId)
        {
            return await _Context.Orders
                .Include(o => o.Product)
                    .ThenInclude(p => p.Owner)
                .Include(o => o.Customer)
                .Where(p => p.PublicId == publicId)
                .FirstOrDefaultAsync();
        }

        public async Task<Order?> GetOrderFromIdAsync(int id)
        {
            return await _Context.Orders
                .Include(o => o.Product)
                    .ThenInclude(p => p.Owner)
                .Include(o => o.Customer)
                .Where(p => p.Id == id)
                .FirstOrDefaultAsync();
        }
    }

    public enum OrderType
    {
        Placed,
        Received
    }
}
