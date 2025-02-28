
namespace FoodDelivery.Features.Food.GetAllFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/foods", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            async (ISender sender, string? name, string? category, decimal? minPrice, decimal? maxPrice, double? minRating, int pageSize = 10, int page = 1, int commentToShow = 1) => {
                return Results.Ok(await sender.Send(new Query {
                    Category = category,
                    MaxPrice = maxPrice,
                    MinPrice = minPrice,
                    MinRating = minRating,
                    Name = name,
                    CommentsToShow = commentToShow,
                    PageNumber = page,
                    PageSize = pageSize
                }));
            }
        )
        .WithName("GetAllFood")
        .WithTags("Food")
        .WithOpenApi(operation => new(operation) {
            Summary = "Get menu",
            OperationId = "GetAllFood",
            Description = "Returns all foods currently in the menu"
        });
    }
}