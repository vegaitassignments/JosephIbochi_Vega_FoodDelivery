
namespace FoodDelivery.Features.Order.GetAllOrders;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/orders", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (ISender sender) => {
                return Results.Ok();
            })
            .WithName("GetAllOrders")
            .WithTags("Order")
            .WithOpenApi(operation => new(operation) {
                Summary = "Get all orders",
                OperationId = "GetAllOrders",
                // Description = "Allows the user to cancel an order before the set time of 15 mins expires"
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK);
    }
}