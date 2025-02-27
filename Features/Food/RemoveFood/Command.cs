
namespace FoodDelivery.Features.Food.RemoveFood;

public class Command : IRequest<BaseResponse>
{
    public int foodId { get; set; }
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
        var food = await _context.Foods.FindAsync(request.foodId) ?? throw new FoodDeliveryNotFoundException("Food not found");
        
        _context.Foods.Remove(food);
        await _context.SaveChangesAsync(cancellationToken);
        
        return new BaseResponse {
            Message = "Food successfully deleted"
        };
    }
}