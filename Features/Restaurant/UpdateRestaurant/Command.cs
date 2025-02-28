
namespace FoodDelivery.Features.Restaurant.UpdateRestaurant;

public class Command : IRequest<BaseResponse>
{
    public UpdateRestaurantDTO requestData { get; set; }
    public int restaurantId { get; set; }
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
        var restaurant = await _context.Restaurants
            .Include(r => r.Courier)
            .FirstOrDefaultAsync(r => r.Id == request.restaurantId, cancellationToken) ?? throw new FoodDeliveryNotFoundException("Restaurant not found");

        var updateData = request.requestData;

        if (!string.IsNullOrEmpty(updateData.Name))
            restaurant.Name = updateData.Name;

        if (updateData.Latitude.HasValue)
            restaurant.Latitude = updateData.Latitude.Value;

        if (updateData.Longitude.HasValue)
            restaurant.Longitude = updateData.Longitude.Value;

        if (!string.IsNullOrEmpty(updateData.CourierName))
            restaurant.Courier.Name = updateData.CourierName;

        if (!string.IsNullOrEmpty(updateData.CourierPhoneNumber))
            restaurant.Courier.PhoneNumber = updateData.CourierPhoneNumber;
        if (!string.IsNullOrEmpty(updateData.CourierPhoneNumber))
            restaurant.Courier.PhoneNumber = updateData.CourierPhoneNumber;

        await _context.SaveChangesAsync(cancellationToken);

        return new BaseResponse
        {
            Status = true,
            Message = "Restaurant updated successfully"
        };
    }
}
