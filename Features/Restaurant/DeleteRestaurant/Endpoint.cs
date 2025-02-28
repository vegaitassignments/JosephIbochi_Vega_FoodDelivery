namespace FoodDelivery.Features.Restaurant.DeleteRestaurant;

public class Endpoint: ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/restaurants/{id}", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")] 
            async (ISender sender, int id) => 
            {
                return Results.Ok(await sender.Send(new Command {restaurantId = id}));
            }
        )
        .WithName("DeleteRestaurant")
        .WithTags("Restaurant")
        .WithOpenApi(operation => new(operation) {
            Summary = "Delete restaurant",
            OperationId = "DeleteRestaurant",
            Description = "Allows to remove a restaurant"
        });
    }
}

// 404 200