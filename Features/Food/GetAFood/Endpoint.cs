
namespace FoodDelivery.Features.Food.GetAFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/foods/{id}", 
            // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (ISender sender, int id) => {
                return Results.Ok(await sender.Send(new Query {foodId = id}));
            }
        )
        .WithName("GetAFood")
        .WithTags("Food")
        .WithOpenApi(operation => new(operation) {
            Summary = "Get a food on the menu",
            OperationId = "GetAFood",
            Description = "Returns details of a food"
        });
    }
}