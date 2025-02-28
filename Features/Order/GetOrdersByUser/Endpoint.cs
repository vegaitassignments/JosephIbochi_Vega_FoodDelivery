
namespace FoodDelivery.Features.Order.GetOrdersByUser;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/orders/users/{id}", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (ISender sender, int id) => {
                return Results.Ok(await sender.Send(new Query()));
            })
            .WithName("GetOrderByUserId")
            .WithTags("Order")
            .WithOpenApi(operation => new(operation) {
                Summary = "Get all orders for a user",
                OperationId = "GetOrderByUserId",
                Description = "Returns a history of all orders, plcaed by a user"
            });
    }
}