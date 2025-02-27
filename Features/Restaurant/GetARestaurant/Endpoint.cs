
namespace FoodDelivery.Features.Restaurant.GetARestaurant;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/restaurants/{id}", 
            // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (ISender sender, int id) => 
            {
                return Results.Ok(await sender.Send(new Query {restaurantId = id}));
            }
        )
        .WithName("GetARestaurant")
        .WithTags("Restaurant")
        .WithOpenApi(operation => new(operation) {
            Summary = "Get a restaurant restaurant",
            OperationId = "GetARestaurant",
            Description = "Returns restaurant details by id"
        });
    }
}
// 200 and 404