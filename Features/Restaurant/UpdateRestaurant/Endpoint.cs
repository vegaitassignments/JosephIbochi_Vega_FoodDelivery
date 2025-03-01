using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Features.Restaurant.UpdateRestaurant;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/restaurants/{id}", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
            async (ISender sender, IValidator<UpdateRestaurantDTO> validator, int id, [FromBody] UpdateRestaurantDTO requestData) => 
            {
                var validationResult = await validator.ValidateAsync(requestData);
                if (validationResult.IsValid) 
                {
                    return Results.Ok(await sender.Send(new Command { requestData = requestData, restaurantId = id }));
                }

                return Results.BadRequest(new BaseResponse 
                {
                    ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage),
                    Status = false,
                    Message = "Failed to update restaurant"
                });
            }
        )
        .WithName("UpdateRestaurant")
        .WithTags("Restaurant")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Update restaurant";
            operation.OperationId = "UpdateRestaurant";
            operation.Description = "Allows admin to update a restaurant.";

            // Request Body Example
            operation.RequestBody = new OpenApiRequestBody
            {
                Description = "Update restaurant details",
                Required = true,
                Content = new Dictionary<string, OpenApiMediaType>
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["name"] = new OpenApiString("New Restaurant Name"),
                            ["latitude"] = new OpenApiDouble(40.7128),
                            ["longitude"] = new OpenApiDouble(-74.0060),
                            ["courierName"] = new OpenApiString("John Doe"),
                            ["courierPhoneNumber"] = new OpenApiString("+12345678901")
                        }
                    }
                }
            };

            // 200 - Success Response
            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Restaurant updated successfully",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["message"] = new OpenApiString("Restaurant updated successfully")
                        }
                    }
                }
            };

            // 400 - Validation Error Response
            operation.Responses["400"] = new OpenApiResponse
            {
                Description = "Validation errors",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(false),
                            ["message"] = new OpenApiString("Failed to update restaurant"),
                            ["validationErrors"] = new OpenApiArray
                            {
                                new OpenApiString("Restaurant Name cannot be empty"),
                                new OpenApiString("Latitude must be between -90 and 90"),
                                new OpenApiString("Invalid phone number format")
                            }
                        }
                    }
                }
            };

            // 404 - Not Found Response
            operation.Responses["404"] = new OpenApiResponse
            {
                Description = "Restaurant not found",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiInteger(404),
                            ["title"] = new OpenApiString("Not Found"),
                            ["detail"] = new OpenApiString("The restaurant with the specified ID was not found.")
                        }
                    }
                }
            };

            return operation;
        });
    }
}
