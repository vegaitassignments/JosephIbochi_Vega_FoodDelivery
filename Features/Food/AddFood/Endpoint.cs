namespace FoodDelivery.Features.Food.AddFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/foods",
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
            async (ISender sender, IValidator<CreateFoodDTO> validator, CreateFoodDTO requestData) =>
            {
                var validationResult = validator.Validate(requestData);
                if (validationResult.IsValid)
                {
                    return Results.Ok(await sender.Send(new Command { requestData = requestData }));
                }

                return Results.BadRequest(new BaseResponse
                {
                    ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage),
                    Status = false,
                    Message = "Failed to create food"
                });
            }
        )
        .WithName("AddFood")
        .WithTags("Food")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Add food to the menu";
            operation.OperationId = "AddFood";
            operation.Description = "Allows an admin to add a food item to the menu.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Food successfully added",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["message"] = new OpenApiString("Food successfully added to the menu"),
                            ["data"] = new OpenApiString("Chicken")
                        }
                    }
                }
            };

            operation.Responses["400"] = new OpenApiResponse
            {
                Description = "Validation error - Missing or invalid fields",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(false),
                            ["message"] = new OpenApiString("Failed to create food"),
                            ["validationErrors"] = new OpenApiArray
                            {
                                new OpenApiString("Food name is required."),
                                new OpenApiString("Price must be greater than zero.")
                            }
                        }
                    }
                }
            };

            return operation;
        });
    }
}