
namespace FoodDelivery.Features.Order.GetAnOrder;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/orders/{id}", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] async (ISender sender, int id) => {
                return Results.Ok();
            })
            .WithName("GetAnOrder")
            .WithTags("Order")
            .WithOpenApi(operation => new(operation) {
                Summary = "Get an order",
                OperationId = "GetAnOrder",
                Description = "Returns an order details"
            })
            .Produces(StatusCodes.Status400BadRequest)
            .Produces(StatusCodes.Status200OK);
    }
}