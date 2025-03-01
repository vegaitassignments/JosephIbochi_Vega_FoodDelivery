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
        .WithOpenApi(operation =>
        {
            operation.Summary = "Delete restaurant";
            operation.OperationId = "DeleteRestaurant";
            operation.Description = "Allows an admin to delete a restaurant by ID.";

            operation.Parameters = new List<OpenApiParameter>
            {
                new()
                {
                    Name = "id",
                    In = ParameterLocation.Path,
                    Required = true,
                    Description = "The ID of the restaurant to delete.",
                    Schema = new OpenApiSchema { Type = "integer", Example = new OpenApiInteger(5) }
                }
            };

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Restaurant deleted successfully",
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Schema = new OpenApiSchema
                        {
                            Type = "object",
                            Properties = new Dictionary<string, OpenApiSchema>
                            {
                                ["status"] = new OpenApiSchema { Type = "boolean", Example = new OpenApiBoolean(true) },
                                ["message"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Restaurant deleted successfully") }
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
                                ["detail"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Restaurant Not Found") }
                            }
                        }
                    }
                }
            };

            return operation;
        });
    }
}
