
namespace FoodDelivery.Features.Order.GetAllOrders;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/orders", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]  
            async (ISender sender, int page = 1, int pageSize = 10) => {
                return Results.Ok(await sender.Send(new Query {Page = page, PageSize = pageSize}));
            }
        )
        .WithName("GetAllOrders")
        .WithTags("Order")
        .WithOpenApi(operation => new(operation) {
            Summary = "Get all orders",
            OperationId = "GetAllOrders",
        });
    }
}