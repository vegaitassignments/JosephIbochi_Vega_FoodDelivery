

using System.Security.Claims;
using FoodDelivery.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Features.Order.PlaceOrder;

public class Command : IRequest<BaseResponse>
{
    public PlaceOrderDTO requestData { get; set; }
}

public class CommandHandler : IRequestHandler<Command, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    public CommandHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    public async Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new FoodDeliveryUnAuthorizedException("Unauthorized");

        var user = await _userManager.FindByIdAsync(userId) ?? throw new FoodDeliveryUnAuthorizedException("User not found");
        var userRole = await _userManager.GetRolesAsync(user);

        foreach (var role in userRole) {
            if (role == "Admin" && (!user.Latitude.HasValue || !user.Longitude.HasValue)){
                throw new FoodDeliveryBadRequestException("When creating this admin user, no latiutude or longitude were provided. Hence closest restaurant cannot be computed");
            }
        }

        var restaurants = await _context.Restaurants
            .Include(o => o.Courier)
            .Where(r => r.IsLockedUntil <= DateTime.UtcNow)
            .ToListAsync();

        var restaurant = restaurants
            .OrderBy(r => GetDistance((double)user.Latitude, (double)user.Longitude, r.Latitude, r.Longitude))
            .FirstOrDefault()
            
            ?? throw new FoodDeliveryBadRequestException("No available restaurant nearby");

        decimal totalPrice = 0;
        var orderItems = new List<OrderItem>();

        foreach(var item in request.requestData.OrderItems)
        {
            var food = await _context.Foods.FindAsync(item.FoodId) ?? throw new FoodDeliveryNotFoundException($"Food with id {item.FoodId} not found");
            var itemTotalPrice = food.Price * item.Quantity;
            totalPrice += itemTotalPrice;

            orderItems.Add(new OrderItem {
                FoodId = item.FoodId,
                ItemPrice = food.Price,
                Quantity = item.Quantity
            });
        }

        var order = new Entities.Order {
            UserId = Guid.Parse(userId),
            CourierId = restaurant.Courier.Id,
            CreatedAt = DateTime.UtcNow,
            OrderItems = orderItems,
            RestaurantId = restaurant.Id,
            Status = Status.InProgress,
            TotalPrice = totalPrice
        };

        _context.Orders.Add(order);

        restaurant.IsLockedUntil = DateTime.UtcNow.AddMinutes(15);

        await _context.SaveChangesAsync();

        var orderResponse = new PlaceOrderResponseDTO (
            order.Id,
            user.Name,
            order.TotalPrice,
            order.Status.ToString(),
            order.CreatedAt,
            order.Restaurant.Name 
        );

        return new BaseResponse {
            Data = orderResponse,
            Status = true,
            Message = "Order successfully placed"
        };
    }

    private double GetDistance(double lat1, double lon1, double lat2, double lon2)
    {
        const double EarthRadiusKm = 6371; // Radius of Earth in km
        double dLat = (lat2 - lat1) * (Math.PI / 180);
        double dLon = (lon2 - lon1) * (Math.PI / 180);

        double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(lat1 * (Math.PI / 180)) * Math.Cos(lat2 * (Math.PI / 180)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

        double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
        return EarthRadiusKm * c; // Returns distance in km
    }
}
