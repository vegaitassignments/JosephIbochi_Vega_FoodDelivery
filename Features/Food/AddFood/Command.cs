
namespace FoodDelivery.Features.Food.AddFood;

public class Command: IRequest<BaseResponse>
{
    public CreateFoodDTO requestData { get; set; }
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
        var data = request.requestData;

        var food = new Entities.Food {
            Category = data.Category,
            Description = data.Description ?? string.Empty,
            ImageUrl = data.ImageUrl ?? string.Empty,
            Name = data.Name,
            Price = data.Price
        };

        _context.Foods.Add(food);
        await _context.SaveChangesAsync(cancellationToken);

        return new BaseResponse {
            Data = food.Name,
            Status = true,
            Message = "Food successfully added to the menu"
        };
    }
}
