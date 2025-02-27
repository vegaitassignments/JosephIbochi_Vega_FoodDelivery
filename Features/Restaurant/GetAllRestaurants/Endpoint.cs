
namespace FoodDelivery.Features.Restaurant.GetAllRestaurants;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/restaurants", 
            // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (ISender sender) => {
                return Results.Ok(await sender.Send(new Query()));
            })
            .WithName("GetAllRestaurants")
            .WithTags("Restaurant")
            .WithOpenApi(operation => new(operation) {
                Summary = "Get all restaurants",
                OperationId = "GetAllRestaurants",
                Description = "Returns all restaurants in the restaurant chain"
            });
    }
}

// 200