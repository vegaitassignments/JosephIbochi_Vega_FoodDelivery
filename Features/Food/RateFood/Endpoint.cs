
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Features.Food.RateFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/foods/{id}/rate", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (ISender sender, IValidator<RateFoodDTO> validator, int id, [FromBody] RateFoodDTO requestData) => {
                var validationResult = validator.Validate(requestData);
                if (validationResult.IsValid) {
                    return Results.Ok(await sender.Send(new Command{requestData = requestData, FoodId = id}));
                }

                return Results.BadRequest(new BaseResponse {
                    ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage),
                    Status = false,
                    Message = "Failed to add rating"
                });
            }
        )
        .WithName("RateFood")
        .WithTags("Food")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Rate food";
            operation.OperationId = "RateFood";
            operation.Description = "Allows a user to rate and comment on a food item.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Successfully added or updated rating",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["message"] = new OpenApiString("Rating added successfully"),
                            ["data"] = new OpenApiObject
                            {
                                ["averageRating"] = new OpenApiDouble(4.5)
                            }
                        }
                    }
                }
            };

            operation.Responses["400"] = new OpenApiResponse
            {
                Description = "Validation failed",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(false),
                            ["message"] = new OpenApiString("Failed to add rating"),
                            ["validationErrors"] = new OpenApiArray
                            {
                                new OpenApiString("Rating must be between 1 and 5"),
                                new OpenApiString("Comment cannot be empty")
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