using FoodDelivery.Entities;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace FoodDelivery.Features.Restaurant.GetOrdersByRestaurant;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/restaurants/{id}/orders",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
            async (ISender sender, int id, Status? status) =>
            {
                return Results.Ok(await sender.Send(new Query { RestaurantId = id, Status = status }));
            }
        )
        .WithName("GetOrdersByRestaurant")
        .WithTags("Restaurant")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get restaurant orders";
            operation.OperationId = "GetOrdersByRestaurant";
            operation.Description = "Returns order details for a specific restaurant.";

            // 200 - Success Response
            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Orders retrieved successfully",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["data"] = new OpenApiArray
                            {
                                new OpenApiObject
                                {
                                    ["orderId"] = new OpenApiInteger(101),
                                    ["userName"] = new OpenApiString("Alice Johnson"),
                                    ["totalPrice"] = new OpenApiDouble(49.99),
                                    ["status"] = new OpenApiString("Completed"),
                                    ["createdAt"] = new OpenApiString("2024-02-28T14:30:00Z")
                                },
                                new OpenApiObject
                                {
                                    ["orderId"] = new OpenApiInteger(102),
                                    ["userName"] = new OpenApiString("Bob Smith"),
                                    ["totalPrice"] = new OpenApiDouble(29.99),
                                    ["status"] = new OpenApiString("Pending"),
                                    ["createdAt"] = new OpenApiString("2024-02-27T10:15:00Z")
                                }
                            }
                        }
                    }
                }
            };

            // 404 - Not Found Response
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