namespace FoodDelivery.Features.Order.GetAllOrders;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet(
            "/orders",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
            async (ISender sender, int page = 1, int pageSize = 10) =>
            {
                return Results.Ok(await sender.Send(new Query { Page = page, PageSize = pageSize }));
            }
        )
        .WithName("GetAllOrders")
        .WithTags("Order")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Get all orders";
            operation.OperationId = "GetAllOrders";
            operation.Description = "Allows an admin to retrieve a paginated list of all orders.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Paginated list of orders",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["data"] = new OpenApiObject
                            {
                                ["totalOrders"] = new OpenApiInteger(50),
                                ["page"] = new OpenApiInteger(1),
                                ["pageSize"] = new OpenApiInteger(10),
                                ["orders"] = new OpenApiArray
                                {
                                    new OpenApiObject
                                    {
                                        ["orderId"] = new OpenApiInteger(123),
                                        ["userName"] = new OpenApiString("John Doe"),
                                        ["totalPrice"] = new OpenApiDouble(29.99),
                                        ["status"] = new OpenApiString("InProgress"),
                                        ["createdAt"] = new OpenApiString("2024-02-28T12:30:00Z"),
                                        ["restaurantName"] = new OpenApiString("Pizza Palace")
                                    }
                                }
                            }
                        }
                    }
                }
            };

            operation.Responses["401"] = new OpenApiResponse
            {
                Description = "Unauthorized - Admin access required",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiInteger(401),
                            ["title"] = new OpenApiString("Unauthorized"),
                            ["detail"] = new OpenApiString("You are not authorized to access this resource.")
                        }
                    }
                }
            };

            return operation;
        });
    }
}