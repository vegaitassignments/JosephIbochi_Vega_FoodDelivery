namespace FoodDelivery.Features.Food.GetAFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/foods/{id}",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            async (ISender sender, int id) =>
            {
                return Results.Ok(await sender.Send(new Query { foodId = id }));
            }
        )
        .WithName("GetAFood")
        .WithTags("Food")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get a food on the menu";
            operation.OperationId = "GetAFood";
            operation.Description = "Returns details of a food item.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Successfully retrieved food details",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["data"] = new OpenApiObject
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
                    }
                }
            };

            operation.Responses["400"] = new OpenApiResponse
            {
                Description = "Food not found",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(false),
                            ["message"] = new OpenApiString("Food not found")
                        }
                    }
                }
            };

            return operation;
        });
    }
}