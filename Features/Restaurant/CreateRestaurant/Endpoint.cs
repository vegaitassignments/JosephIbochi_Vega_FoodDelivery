
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace FoodDelivery.Features.Restaurant.CreateRestaurant;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/restaurants", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")] 
            async (ISender sender, IValidator<CreateRestaurantDTO> validator, [FromBody] CreateRestaurantDTO requestData
        ) => {
                var validationResult = await validator.ValidateAsync(requestData);

                if (validationResult.IsValid){
                    return Results.Ok(await sender.Send(new Command {reequestData = requestData}));
                }

                return Results.BadRequest(new BaseResponse {
                    ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage),
                    Status = false,
                    Message = "Failed to create a restaurant"
                }); 
            })
            .WithName("CreateRestaurant")
            .WithTags("Restaurant")
            .WithOpenApi(operation =>
            {
            operation.Summary = "Create a new restaurant";
            operation.OperationId = "CreateRestaurant";
            operation.Description = "Allows an admin to create a restaurant with an associated courier.";

            
                operation.RequestBody = new OpenApiRequestBody
                {
                    Description = "Restaurant details required for creation",
                    Required = true,
                    Content =
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Schema = new OpenApiSchema
                            {
                                Type = "object",
                                Properties =
                                {
                                    ["name"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("Pizza Palace") },
                                    ["latitude"] = new OpenApiSchema { Type = "number", Format = "double", Example = new OpenApiDouble(40.7128) },
                                    ["longitude"] = new OpenApiSchema { Type = "number", Format = "double", Example = new OpenApiDouble(-74.0060) },
                                    ["courier"] = new OpenApiSchema
                                    {
                                        Type = "object",
                                        Properties =
                                        {
                                            ["name"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("John Doe") },
                                            ["phoneNumber"] = new OpenApiSchema { Type = "string", Example = new OpenApiString("+1234567890") }
                                        }
                                    }
                                }
                            },
                            Example = new OpenApiObject
                            {
                                ["name"] = new OpenApiString("Pizza Palace"),
                                ["latitude"] = new OpenApiDouble(40.7128),
                                ["longitude"] = new OpenApiDouble(-74.0060),
                                ["courier"] = new OpenApiObject
                                {
                                    ["name"] = new OpenApiString("John Doe"),
                                    ["phoneNumber"] = new OpenApiString("+1234567890")
                                }
                            }
                        }
                    }
                };

                operation.Responses["200"] = new OpenApiResponse
                {
                    Description = "Restaurant created successfully.",
                    Content =
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Example = new OpenApiObject
                            {
                                ["status"] = new OpenApiBoolean(true),
                                ["message"] = new OpenApiString("Restaurant created successfully."),
                                ["data"] = new OpenApiObject
                                {
                                    ["id"] = new OpenApiInteger(1),
                                    ["name"] = new OpenApiString("Pizza Palace"),
                                    ["latitude"] = new OpenApiDouble(40.7128),
                                    ["longitude"] = new OpenApiDouble(-74.0060),
                                    ["courier"] = new OpenApiObject
                                    {
                                        ["id"] = new OpenApiInteger(101),
                                        ["name"] = new OpenApiString("John Doe"),
                                        ["phoneNumber"] = new OpenApiString("+1234567890")
                                    }
                                }
                            }
                        }
                    }
                };

                operation.Responses["400"] = new OpenApiResponse
                {
                    Description = "Invalid request data",
                    Content =
                    {
                        ["application/json"] = new OpenApiMediaType
                        {
                            Example = new OpenApiObject
                            {
                                ["status"] = new OpenApiBoolean(false),
                                ["message"] = new OpenApiString("Failed to create a restaurant."),
                                ["validationErrors"] = new OpenApiArray
                                {
                                    new OpenApiString("Name is required"),
                                    new OpenApiString("Latitude must be between -90 and 90"),
                                    new OpenApiString("Longitude must be between -180 and 180"),
                                    new OpenApiString("Courier name is required")
                                }
                            }
                        }
                    }
                };

                return operation;
        });
    }
}