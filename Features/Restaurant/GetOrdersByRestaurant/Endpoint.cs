
using FoodDelivery.Entities;

namespace FoodDelivery.Features.Restaurant.GetOrdersByRestaurant;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/restaurants/{id}/orders", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")] 
            async (ISender sender, int id, Status? status) => {
                return Results.Ok(await sender.Send(new Query {RestaurantId = id, Status = status}));
            }
        )
        .WithName("GetOrdersByRestaurant")
        .WithTags("Restaurant")
        .WithOpenApi(operation => new(operation) {
            Summary = "Get restaurant orders",
            OperationId = "GetOrdersByRestaurant",
            Description = "Returns an order details"
        });
    }
}