

using System.Security.Claims;
using FoodDelivery.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Features.Order.CancelOrder;

public class Command : IRequest<BaseResponse>
{
    public int OrderId { get; set; }
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
        var isAdmin = userRole.Contains("Admin");

        var order = await _context.Orders.FindAsync(request.OrderId) ?? throw new FoodDeliveryNotFoundException("Order not found.");

        if (order.Status != Status.InProgress)
        {
            throw new FoodDeliveryBadRequestException("Only in progress orders can be cancelled.");
        }

        if (order.UserId != Guid.Parse(userId) && !isAdmin)
        {
            throw new FoodDeliveryNotFoundException("Order not valid for you");
        }

        order.Status = Status.Cancelled;
        var restaurant = await _context.Restaurants.FindAsync(order.RestaurantId);
        restaurant.IsLockedUntil = DateTime.UtcNow;
        
         
        await _context.SaveChangesAsync();
        return new BaseResponse {
            Message = "Order successfully cancelled",
            Status = true
        };
    }
}
