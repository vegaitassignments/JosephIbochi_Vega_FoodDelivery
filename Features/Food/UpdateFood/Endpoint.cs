using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Features.Food.UpdateFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/foods/{id}", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")] 
            async (ISender sender, IValidator<UpdateFoodDTO> validator, int id, [FromBody] UpdateFoodDTO requestData) =>
            {
                var validationResult = await validator.ValidateAsync(requestData);

                if (validationResult.IsValid)
                {
                    return Results.Ok(await sender.Send(new Command { FoodId = id, RequestData = requestData }));
                }

                return Results.BadRequest(new BaseResponse
                {
                    ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage),
                    Status = false,
                    Message = "Failed to update food"
                });
            }
        )
        .WithName("UpdateFood")
        .WithTags("Food")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Edit food";
            operation.OperationId = "UpdateFood";
            operation.Description = "Allows an admin to update an existing food item on the menu.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Food updated successfully",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["message"] = new OpenApiString("Food updated successfully"),
                            ["data"] = new OpenApiObject
                            {
                                ["name"] = new OpenApiString("Spaghetti Carbonara"),
                                ["category"] = new OpenApiString("Italian"),
                                ["price"] = new OpenApiDouble(12.99),
                                ["imageUrl"] = new OpenApiString("https://example.com/spaghetti.jpg"),
                                ["description"] = new OpenApiString("Delicious creamy spaghetti with bacon and eggs.")
                            }
                        }
                    }
                }
            };

            operation.Responses["400"] = new OpenApiResponse
            {
                Description = "Validation error - Invalid or missing fields",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(false),
                            ["message"] = new OpenApiString("Failed to update food"),
                            ["validationErrors"] = new OpenApiArray
                            {
                                new OpenApiString("Food name cannot be empty."),
                                new OpenApiString("Price must be greater than 0.")
                            }
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