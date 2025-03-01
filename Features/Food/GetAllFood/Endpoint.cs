namespace FoodDelivery.Features.Food.GetAllFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/foods",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            async (ISender sender, string? name, string? category, decimal? minPrice, decimal? maxPrice, double? minRating, int pageSize = 10, int page = 1, int commentToShow = 1) =>
            {
                return Results.Ok(await sender.Send(new Query
                {
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
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get menu";
            operation.OperationId = "GetAllFood";
            operation.Description = "Returns all foods currently in the menu, with optional filtering by name, category, price, and rating.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Successfully retrieved food list",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["data"] = new OpenApiObject
                            {
                                ["totalRecords"] = new OpenApiInteger(50),
                                ["pageNumber"] = new OpenApiInteger(1),
                                ["pageSize"] = new OpenApiInteger(10),
                                ["foods"] = new OpenApiArray
                                {
                                    new OpenApiObject
                                    {
                                        ["id"] = new OpenApiInteger(1),
                                        ["name"] = new OpenApiString("Chicken Drumstick"),
                                        ["price"] = new OpenApiDouble(12.99),
                                        ["imageUrl"] = new OpenApiString("https://example.com/chicken.jpg"),
                                        ["description"] = new OpenApiString("Crispy fried chicken drumstick"),
                                        ["averageRating"] = new OpenApiDouble(4.5),
                                        ["category"] = new OpenApiString("Fast Food"),
                                        ["foodRatings"] = new OpenApiArray
                                        {
                                            new OpenApiObject
                                            {
                                                ["rating"] = new OpenApiInteger(5),
                                                ["comment"] = new OpenApiString("Amazing taste!"),
                                                ["createdAt"] = new OpenApiString("2024-02-28T12:34:56Z")
                                            }
                                        }
                                    }
                                }
                            },
                            ["message"] = new OpenApiString("Restaurant chain menu")
                        }
                    }
                }
            };

            return operation;
        });
    }
}