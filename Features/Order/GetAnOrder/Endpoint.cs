namespace FoodDelivery.Features.Order.GetAnOrder;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/orders/{id}",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            async (ISender sender, int id) =>
            {
                return Results.Ok(await sender.Send(new Query { OrderId = id }));
            })
            .WithName("GetAnOrder")
            .WithTags("Order")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Get an order";
                operation.OperationId = "GetAnOrder";
                operation.Description = "Returns the details of a specific order.";

                operation.Responses["200"] = new OpenApiResponse
                {
                    Description = "Order details retrieved successfully",
                    Content =
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Example = new OpenApiObject
                            {
                                ["status"] = new OpenApiBoolean(true),
                                ["data"] = new OpenApiObject
                                {
                                    ["id"] = new OpenApiInteger(123),
                                    ["totalPrice"] = new OpenApiDouble(29.99),
                                    ["status"] = new OpenApiString("Completed"),
                                    ["createdAt"] = new OpenApiString("2024-02-28T12:30:00Z"),
                                    ["restaurantId"] = new OpenApiInteger(5),
                                    ["restaurantName"] = new OpenApiString("Pizza Palace"),
                                    ["orderItems"] = new OpenApiArray
                                    {
                                        new OpenApiObject
                                        {
                                            ["foodId"] = new OpenApiInteger(10),
                                            ["foodName"] = new OpenApiString("Margherita Pizza"),
                                            ["itemPrice"] = new OpenApiDouble(15.99),
                                            ["quantity"] = new OpenApiInteger(2)
                                        }
                                    }
                                }
                            }
                        }
                    }
                };

                operation.Responses["400"] = new OpenApiResponse
                {
                    Description = "Invalid request - Order ID must be valid",
                    Content =
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Example = new OpenApiObject
                            {
                                ["status"] = new OpenApiInteger(400),
                                ["title"] = new OpenApiString("Bad Request"),
                                ["detail"] = new OpenApiString("The order ID provided is invalid.")
                            }
                        }
                    }
                };

                operation.Responses["404"] = new OpenApiResponse
                {
                    Description = "Order not found",
                    Content =
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Example = new OpenApiObject
                            {
                                ["status"] = new OpenApiInteger(404),
                                ["title"] = new OpenApiString("Not Found"),
                                ["detail"] = new OpenApiString("The order with the specified ID was not found.")
                            }
                        }
                    }
                };

                return operation;
            });
    }
}