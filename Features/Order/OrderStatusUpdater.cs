using FoodDelivery.Entities;

namespace FoodDelivery.Features.Order;

public class OrderStatusUpdater
{
    private readonly ApplicationDbContext _context;
    public OrderStatusUpdater(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task UpdateOrderStatuses() 
    {
        var now =DateTime.UtcNow;                
        var deliveryTime = TimeSpan.FromMinutes(15);

        var ordersForUpdate = _context.Orders
            .AsEnumerable() 
            .Where(o => o.Status == Status.InProgress && now >= o.CreatedAt.Add(deliveryTime))
            .ToList();

        foreach(var order in ordersForUpdate)
        {
            order.Status = Status.Delivered;

        }

        if (ordersForUpdate.Any()){
            
            await _context.SaveChangesAsync();
        }
    }
}