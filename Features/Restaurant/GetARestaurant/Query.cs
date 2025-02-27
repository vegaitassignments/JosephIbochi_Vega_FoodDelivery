
namespace FoodDelivery.Features.Restaurant.GetARestaurant;

public class Query : IRequest<BaseResponse>
{
    public int restaurantId { get; set; }
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
        var restaurant = await _context.Restaurants
            .Include(r => r.Courier)
            .FirstOrDefaultAsync(r => r.Id == request.restaurantId);

        if (restaurant == null){
            throw new FoodDeliveryNotFoundException("Restaurant does not exist");
        }

        var data = new GetRestaurantDTO(
            restaurant.Name,
            restaurant.Latitude,
            restaurant.Longitude,
            restaurant.Courier.Name,
            restaurant.Courier.PhoneNumber,
            restaurant.isEngaged
        );

        return new BaseResponse {
            Data = data,
            Status = true
        };
    }
}
