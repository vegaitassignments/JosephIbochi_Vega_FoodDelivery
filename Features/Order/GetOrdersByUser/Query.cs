
using System.Security.Claims;
using FoodDelivery.Entities;
using FoodDelivery.Features.Order.GetAnOrder;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Features.Order.GetOrdersByUser;

public class Query : IRequest<BaseResponse>
{
    
}

public class QueryHandler : IRequestHandler<Query, BaseResponse>
{

    private readonly ApplicationDbContext _context;
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly UserManager<ApplicationUser> _userManager;
    public QueryHandler(ApplicationDbContext context, IHttpContextAccessor contextAccessor, UserManager<ApplicationUser> userManager)
    {
        _context = context;
        _contextAccessor = contextAccessor;
        _userManager = userManager;
    }

    public async Task<BaseResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var userId = _contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new FoodDeliveryUnAuthorizedException("Unauthorized");

        var orders = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Food)
            .Include(o => o.Restaurant) 
            .Where(o => o.UserId == Guid.Parse(userId))
            .ToListAsync();

        var ordersDTO = orders.Select( orders => new OrderDTO
        (
            orders.Id,
            orders.TotalPrice,
            orders.Status.ToString(),
            orders.CreatedAt,
            orders.RestaurantId,
            orders.Restaurant?.Name,
            orders.OrderItems.Select(oi => new OrderItemDTO
            (
                oi.FoodId,
                oi.Food.Name,
                oi.ItemPrice,
                oi.Quantity
            )).ToList()
        )).ToList();

        return new BaseResponse {
            Data = ordersDTO,
            Status = true

        };

        

    }
}
