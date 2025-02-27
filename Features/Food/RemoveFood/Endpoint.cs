
namespace FoodDelivery.Features.Food.RemoveFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/foods/{id}", 
            // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (ISender sender, int id) => {
                return Results.Ok(await sender.Send(new Command {foodId = id}));
            }
        )
        .WithName("RemoveFood")
        .WithTags("Food")
        .WithOpenApi(operation => new(operation) {
            Summary = "Remove food",
            OperationId = "RemoveFood",
            Description = "Remove food from a menu"
        });
    }
}

// 404 200