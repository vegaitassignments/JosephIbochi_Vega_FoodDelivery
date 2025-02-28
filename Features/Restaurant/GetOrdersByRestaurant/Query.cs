
using FoodDelivery.Entities;

namespace FoodDelivery.Features.Restaurant.GetOrdersByRestaurant;

public class Query : IRequest<BaseResponse>
{
    public Status? Status { get; set; }
    public int RestaurantId { get; set; }
}

public class QueryHandler : IRequestHandler<Query, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    public QueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<BaseResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var restaurant = await _context.Restaurants.FirstOrDefaultAsync(r => r.Id == request.RestaurantId) ?? throw new FoodDeliveryNotFoundException("Restaurant not found");
        
        var query = _context.Orders
            .Where(o => o.RestaurantId == request.RestaurantId);

        
        if (request.Status.HasValue)
        {
            query = query.Where(o => o.Status == request.Status.Value);
        }

        var orders = await query
            .Select(o => new GetLessRestaurantOrdersDetailsDTO(
                o.Id,
                o.User.Name,
                o.TotalPrice,
                o.Status.ToString(),
                o.CreatedAt
            )).ToListAsync();

        return new BaseResponse {
            Data = orders,
            Status = true
        };
    }
}
