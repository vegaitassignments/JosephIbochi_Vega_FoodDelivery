
namespace FoodDelivery.Features.Food.GetAFood;

public class Query : IRequest<BaseResponse>
{
    public int foodId { get; set; }
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
        var food = await _context.Foods
            .Include(f => f.Ratings)
            .FirstOrDefaultAsync(f => f.Id == request.foodId) ?? throw new FoodDeliveryNotFoundException("Food not found");

        var foodDTO = new FoodDTO(
            food.Id,
            food.Name,
            food.Price,
            food.ImageUrl,
            food.Description,
            food.AverageRating,
            food.Category,
            food.Ratings.Select(r => new FoodRatingDTO(
                r.Rating,
                r.Comment,
                r.CreatedAt
            )).ToList()
        );

        return new BaseResponse {
            Data = foodDTO,
            Status = true,
        };
    }
}
