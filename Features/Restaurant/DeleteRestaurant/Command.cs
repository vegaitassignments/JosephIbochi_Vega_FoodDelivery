
namespace FoodDelivery.Features.Restaurant.DeleteRestaurant;

public class Command: IRequest<BaseResponse>
{
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
        var restaurant = await _context.Restaurants.FindAsync(request.restaurantId);
        if (restaurant == null) {
            throw new FoodDeliveryNotFoundException("Restaurant does not exist");
        }

        _context.Remove(restaurant);
        await _context.SaveChangesAsync(cancellationToken);

        return new BaseResponse {
            Status = true,
            Message = "Restaurant deleted successfully"
        };
    }
}
