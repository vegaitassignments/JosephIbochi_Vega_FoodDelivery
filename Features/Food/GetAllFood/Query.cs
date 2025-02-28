
using FoodDelivery.Features.Food.GetAFood;

namespace FoodDelivery.Features.Food.GetAllFood;

public class Query : IRequest<BaseResponse>
{
     public string? Name { get; set; } 
    public string? Category { get; set; } 
    public decimal? MinPrice { get; set; }
    public decimal? MaxPrice { get; set; }
    public double? MinRating { get; set; }
    public int PageNumber { get; set; } = 1; 
    public int PageSize { get; set; } = 10;
    public int CommentsToShow { get; set; } = 1; 
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
        var query = _context.Foods.Include(f => f.Ratings).AsQueryable();

        if (!string.IsNullOrEmpty(request.Name))
        {
            query = query.Where(f => f.Name.Contains(request.Name));
        }

        if (!string.IsNullOrEmpty(request.Category))
        {
            query = query.Where(f => f.Category == request.Category);
        }

        if (request.MinPrice.HasValue)
        {
            query = query.Where(f => f.Price >= request.MinPrice);
        }

        if (request.MaxPrice.HasValue)
        {
            query = query.Where(f => f.Price <= request.MaxPrice);
        }

        var totalRecords = await query.CountAsync();
        var foods = await query
            .OrderBy(f => f.Name)
            .Skip((request.PageNumber - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToListAsync(cancellationToken); 

        if (request.MinRating.HasValue)
        {
            foods = foods.Where(f => f.AverageRating >= request.MinRating.Value).ToList();
        }

        var foodDTOs = foods
            .Select(f => new FoodDTO(
                f.Id,
                f.Name,
                f.Price,
                f.ImageUrl,
                f.Description,
                f.AverageRating,
                f.Category,
                f.Ratings
                    .OrderByDescending(r => r.CreatedAt)
                    .Take(request.CommentsToShow)
                    .Select(r => new FoodRatingDTO(
                        r.Rating,
                        r.Comment,
                        r.CreatedAt
                    ))
                    .ToList()
            )).ToList();

        return new BaseResponse
        {
            Status = true,
            Data = new
            {
                TotalRecords = totalRecords,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                Foods = foodDTOs
            },
            Message = "Restaurant chain menu"
        };
    }
}
