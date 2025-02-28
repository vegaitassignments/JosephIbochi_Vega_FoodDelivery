
namespace FoodDelivery.Features.Restaurant.CreateRestaurant;

public class Command: IRequest<BaseResponse>
{
    public CreateRestaurantDTO reequestData { get; set; }
}

public class CommandHandler : IRequestHandler<Command, BaseResponse>
{
    private readonly ApplicationDbContext _context;
    public CommandHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    
    public async Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        var requestData = request.reequestData;

        var restaurant = new Entities.Restaurant {
            Name = requestData.Name,
            Latitude = requestData.Latitude,
            Longitude = requestData.Longitude,
            IsLockedUntil = null
        };

        _context.Restaurants.Add(restaurant);
        await _context.SaveChangesAsync(cancellationToken);

        var courier = new Entities.Courier {
            Name = requestData.Courier.Name,
            PhoneNumber = requestData.Courier.PhoneNumber,
            RestaurantId = restaurant.Id
        };

        _context.Couriers.Add(courier);
        await _context.SaveChangesAsync(cancellationToken);

        return new BaseResponse {
            Data = new { RestaurantName = restaurant.Name, Courier = courier.Name },
            Status = true,
            Message = "Restaurant and courier created successfully"
        };
    }
}
