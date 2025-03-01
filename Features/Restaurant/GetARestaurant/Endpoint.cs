namespace FoodDelivery.Features.Restaurant.GetARestaurant;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/restaurants/{id}",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
            async (ISender sender, int id) =>
            {
                return Results.Ok(await sender.Send(new Query { restaurantId = id }));
            }
        )
        .WithName("GetARestaurant")
        .WithTags("Restaurant")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get a restaurant";
            operation.OperationId = "GetARestaurant";
            operation.Description = "Returns restaurant details by ID.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Restaurant details retrieved successfully",
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
                                    ["name"] = new OpenApiString("Pizza Palace"),
                                   ["latitude"] = new OpenApiDouble(40.7128),
                                    ["longitude"] = new OpenApiDouble(-74.0060),
                                    ["courierName"] = new OpenApiString("John Doe"),
                                    ["courierPhoneNumber"] = new OpenApiString("+1234567890"),
                                    ["isAvailable"] = new OpenApiBoolean(true)
                                }
                        }
                    }
                }
            };

            operation.Responses["404"] = new OpenApiResponse
            {
                Description = "Restaurant not found",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["status"] = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(404) },
                                ["title"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Not Found") },
                                ["detail"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("The restaurant with the specified ID was not found.") }
                            }
                        }
                    }
                }
            };

            return operation;
        });
    }
}