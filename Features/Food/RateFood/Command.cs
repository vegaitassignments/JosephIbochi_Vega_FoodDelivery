
using System.Security.Claims;
using FoodDelivery.Entities;

namespace FoodDelivery.Features.Food.RateFood;

public class Command : IRequest<BaseResponse>
{
    public int FoodId { get; set; }
    public RateFoodDTO requestData { get; set; }
}

public class CommandHandler : IRequestHandler<Command, BaseResponse>
{
    private readonly IHttpContextAccessor _contextAccessor;
    private readonly ApplicationDbContext _context;
    public CommandHandler(IHttpContextAccessor contextAccessor, ApplicationDbContext context)
    {
        _contextAccessor = contextAccessor;
        _context = context;
    }
    public async Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        var userId = Guid.Parse(_contextAccessor.HttpContext.User.FindFirstValue(ClaimTypes.NameIdentifier) 
            ?? throw new FoodDeliveryUnAuthorizedException("Unauthorized"));

        var food = await _context.Foods.Include(f => f.Ratings)
            .FirstOrDefaultAsync(f => f.Id == request.FoodId) ?? throw new FoodDeliveryNotFoundException("Food not found");
        
        var existingRating = await _context.FoodRatings
            .FirstOrDefaultAsync(r => r.FoodId == request.FoodId && r.UserId == userId);
        if (existingRating  != null) {
            existingRating.Rating = request.requestData.Rating;
            existingRating.Comment = request.requestData.Comment;
        }

        else
        {
            // Add new rating
            var newRating = new FoodRating
            {
                FoodId = request.FoodId,
                UserId = userId,
                Rating = request.requestData.Rating,
                Comment = request.requestData.Comment
            };

            _context.FoodRatings.Add(newRating);
        }

        await _context.SaveChangesAsync(cancellationToken);
        
        return new BaseResponse {
            Message = existingRating != null ? "Rating updated successfully" : "Rating added successfully",
            Data = new {avarageRating = food.AverageRating },
            Status = true 
        };
    }
}
