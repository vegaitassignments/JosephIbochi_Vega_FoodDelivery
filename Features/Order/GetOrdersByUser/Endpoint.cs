namespace FoodDelivery.Features.Order.GetOrdersByUser;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/orders/users/{id}",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            async (ISender sender, int id) =>
            {
                return Results.Ok(await sender.Send(new Query()));
            })
            .WithName("GetOrderByUserId")
            .WithTags("Order")
            .WithOpenApi(operation =>
            {
                operation.Summary = "Get all orders for a user";
                operation.OperationId = "GetOrderByUserId";
                operation.Description = "Returns a history of all orders placed by a specific user.";

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
                                ["data"] =
                                    new OpenApiObject
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

                return operation;
            });
    }
}