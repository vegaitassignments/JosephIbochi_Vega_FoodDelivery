
namespace FoodDelivery.Features.Order.CancelOrder;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/orders/{id}/cancel", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (ISender sender, int id) => {
                return Results.Ok();
            })
            .WithName("CancelOrder")
            .WithTags("Order")
            .WithOpenApi(operation => new(operation) {
                Summary = "Cancel order",
                OperationId = "CancelOrder",
                Description = "Allows the user to cancel an order before the set time of 15 mins expires"
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK);
    }
}