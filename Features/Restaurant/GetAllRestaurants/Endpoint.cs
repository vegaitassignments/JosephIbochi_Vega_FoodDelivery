
namespace FoodDelivery.Features.Restaurant.GetAllRestaurants;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/restaurants", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]  
            async (ISender sender) => {
                return Results.Ok(await sender.Send(new Query()));
            }
        )
        .WithName("GetAllRestaurants")
        .WithTags("Restaurant")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get all restaurants";
            operation.OperationId = "GetAllRestaurants";
            operation.Description = "Returns all restaurants in the restaurant chain.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "List of all restaurants",
            };

            return operation;
        });

    }
}