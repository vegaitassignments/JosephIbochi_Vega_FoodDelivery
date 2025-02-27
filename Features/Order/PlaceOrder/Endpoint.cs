
namespace FoodDelivery.Features.Order.PlaceOrder;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/orders", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (ISender sender) => {
                return Results.Ok();
            })
            .WithName("PlaceOrder")
            .WithTags("Order")
            .WithOpenApi(operation => new(operation) {
                Summary = "Place an order",
                OperationId = "PlaceOrder",
                Description = "Alloes a user to place an order"
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK);
    }
}