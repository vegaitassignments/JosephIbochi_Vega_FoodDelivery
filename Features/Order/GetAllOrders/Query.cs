
namespace FoodDelivery.Features.Order.GetAllOrders;

public class Query : IRequest<BaseResponse>
{
    public int Page { get; set; }
    public int PageSize { get; set; }
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
        var orders = await _context.Orders
            .Select(o => new GetLessRestaurantOrdersDetailsDTO(
                o.Id,
                o.User.Name,
                o.TotalPrice,
                o.Status.ToString(),
                o.CreatedAt,
                o.Restaurant.Name
            )).ToListAsync();

        var totalOrders = orders.Count();
        var paginatedOrders = orders
            .Skip((request.Page - 1) * request.PageSize)
            .Take(request.PageSize)
            .ToList();

        return new BaseResponse {
            Data = new {
                totalOrders =  totalOrders,
                page = request.Page,
                pageSize = request.PageSize,
                orders = paginatedOrders 
            },
            Status = true
        };
    }
}
