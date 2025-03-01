namespace FoodDelivery.Features.Order.CancelOrder;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/orders/{id}/cancel", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
            async (ISender sender, int id) => 
            {
                return Results.Ok(await sender.Send(new Command { OrderId = id }));
            }
        )
        .WithName("CancelOrder")
        .WithTags("Order")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Cancel order";
            operation.OperationId = "CancelOrder";
            operation.Description = "Allows the user to cancel an order before the set time of 15 minutes expires.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Order successfully cancelled",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["message"] = new OpenApiString("Order successfully cancelled")
                        }
                    }
                }
            };

            operation.Responses["400"] = new OpenApiResponse
            {
                Description = "Order is not valid for you",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiInteger(400),
                            ["title"] = new OpenApiString("Bad Request"),
                            ["detail"] = new OpenApiString("The order is not valid for you to cancel.")
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