
namespace FoodDelivery.Features.Order.GetAnOrder;

public class Query : IRequest<BaseResponse>
{
    public int OrderId { get; set; }
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
        var order = await _context.Orders
            .Include(o => o.OrderItems)
            .ThenInclude(oi => oi.Food)
            .Include(o => o.Restaurant) 
            .FirstOrDefaultAsync(o => o.Id == request.OrderId) 
            ?? throw new FoodDeliveryNotFoundException("Order not found");

        var orderDTO = new OrderDTO
        (
            order.Id,
            order.TotalPrice,
            order.Status.ToString(),
            order.CreatedAt,
            order.RestaurantId,
            order.Restaurant?.Name,
            order.OrderItems.Select(oi => new OrderItemDTO
            (
                oi.FoodId,
                oi.Food.Name,
                oi.ItemPrice,
                oi.Quantity
            )).ToList()
        );

        return new BaseResponse {
            Data = orderDTO,
            Status = true
        };
    }   
}
