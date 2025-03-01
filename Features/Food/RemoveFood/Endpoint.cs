namespace FoodDelivery.Features.Food.RemoveFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete(
            "/foods/{id}", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")] 
            async (ISender sender, int id) =>
            {
                return Results.Ok(await sender.Send(new Command { foodId = id }));
            }
        )
        .WithName("RemoveFood")
        .WithTags("Food")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Remove food";
            operation.OperationId = "RemoveFood";
            operation.Description = "Removes a food item from the menu. Only admins can perform this action.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Food successfully deleted",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["message"] = new OpenApiString("Food successfully deleted")
                        }
                    }
                }
            };

            operation.Responses["404"] = new OpenApiResponse
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