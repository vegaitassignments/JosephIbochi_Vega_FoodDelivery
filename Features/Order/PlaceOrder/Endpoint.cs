namespace FoodDelivery.Features.Order.PlaceOrder;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/orders",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            async (ISender sender, PlaceOrderDTO requestData) =>
            {
                return Results.Ok(await sender.Send(new Command { requestData = requestData }));
            }
        )
        .WithName("PlaceOrder")
        .WithTags("Order")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Place an order";
            operation.OperationId = "PlaceOrder";
            operation.Description = "Allows a user to place an order.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Order successfully placed",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["message"] = new OpenApiString("Order successfully placed"),
                            ["data"] = new OpenApiObject
                            {
                                ["orderId"] = new OpenApiInteger(123),
                                ["customerName"] = new OpenApiString("John Doe"),
                                ["totalPrice"] = new OpenApiDouble(49.99),
                                ["status"] = new OpenApiString("InProgress"),
                                ["createdAt"] = new OpenApiString("2024-02-28T14:00:00Z"),
                                ["restaurantName"] = new OpenApiString("Best Burger")
                            }
                        }
                    }
                }
            };

            operation.Responses["400"] = new OpenApiResponse
            {
                Description = "Invalid request - Missing latitude/longitude",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiInteger(400),
                            ["title"] = new OpenApiString("Bad Request"),
                            ["detail"] = new OpenApiString("When creating this admin user, no latitude or longitude were provided. Hence, the closest restaurant cannot be computed.")
                        }
                    }
                }
            };

            operation.Responses["400-Restaurant"] = new OpenApiResponse
            {
                Description = "No available restaurant nearby",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiInteger(400),
                            ["title"] = new OpenApiString("Bad Request"),
                            ["detail"] = new OpenApiString("No available restaurant nearby.")
                        }
                    }
                }
            };

            return operation;
        });
    }
}