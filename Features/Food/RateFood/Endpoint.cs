
namespace FoodDelivery.Features.Food.RateFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/foods/{id}/rate", 
            // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (ISender sender, int id) => {
                return Results.Ok();
            }
        )
        .WithName("RateFood")
        .WithTags("Food")
        .WithOpenApi(operation => new(operation) {
            Summary = "Rate food",
            OperationId = "RateFood",
            Description = "Allows a user to rate and comment on a food"
        });
    }
}

// 400 200 404