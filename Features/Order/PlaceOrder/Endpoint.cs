
namespace FoodDelivery.Features.Order.PlaceOrder;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/orders", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (ISender sender, PlaceOrderDTO requestData) => {
                return Results.Ok(await sender.Send(new Command {requestData = requestData}));
            }
        )
        .WithName("PlaceOrder")
        .WithTags("Order")
        .WithOpenApi(operation => new(operation) {
            Summary = "Place an order",
            OperationId = "PlaceOrder",
            Description = "Allows a user to place an order"
        });
    }
}