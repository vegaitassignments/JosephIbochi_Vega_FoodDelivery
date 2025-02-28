
using FoodDelivery.Features.Restaurant.GetARestaurant;

namespace FoodDelivery.Features.Restaurant.GetAllRestaurants;

public class Query : IRequest<BaseResponse>{}

public class QueryHandler : IRequestHandler<Query, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    public QueryHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<BaseResponse> Handle(Query request, CancellationToken cancellationToken)
    {
        var restaurant = await _context.Restaurants
            .Include(r => r.Courier)
            .ToListAsync(cancellationToken);

        var data = restaurant.Select( r => new GetRestaurantDTO(
            r.Id,
            r.Name,
            r.Latitude,
            r.Longitude,
            r.Courier.Name,
            r.Courier.PhoneNumber,
            r.IsAvailable
        )).ToList();

        return new BaseResponse {
            Data = data,
            Status = true
        };
    }
}
