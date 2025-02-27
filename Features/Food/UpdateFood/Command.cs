
namespace FoodDelivery.Features.Food.UpdateFood;

public class Command : IRequest<BaseResponse>
{
    public int FoodId { get; set; }
    public UpdateFoodDTO RequestData { get; set; }
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
        var food = await _context.Foods.FindAsync(request.FoodId) ?? throw new FoodDeliveryNotFoundException("Food not found");

        var data = request.RequestData;
        if (!string.IsNullOrEmpty(data.Name))
            food.Name = data.Name;

        if (!string.IsNullOrEmpty(data.Category))
            food.Category = data.Category;

        if (data.Price.HasValue)
            food.Price = data.Price.Value;

        if (!string.IsNullOrEmpty(data.ImageUrl))
            food.ImageUrl = data.ImageUrl;

        if (!string.IsNullOrEmpty(data.Description))
            food.Description = data.Description;

        _context.Foods.Update(food);
        await _context.SaveChangesAsync(cancellationToken);

        return new BaseResponse {
            Data = new {
                Name = food.Name,
                Category = food.Category,
                Price = food.Price,
                ImageUrl = food.ImageUrl,
                Description = food.Description
            },
            Message = "Food updated successfully"
        };
    }
}
