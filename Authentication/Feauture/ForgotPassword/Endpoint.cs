using FoodDelivery.Authentication.Feauture.ForgotPassword;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;

namespace FoodDelivery.Authentication.Feature.ForgotPassword;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/forgot-password", 
            async (ISender sender, IValidator<UpdatePasswordDTO> validator, UpdatePasswordDTO requestData) =>
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
                });
            }
        )
        .WithName("ForgotPassword")
        .WithTags("Auth")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Request a password reset token";
            operation.OperationId = "ForgotPassword";
            operation.Description = "Generates a token for password reset and sends it";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Password reset token generated successfully",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["message"] = new OpenApiString("Use this token to reset the password."),
                            ["data"] = new OpenApiObject
                            {
                                ["email"] = new OpenApiString("user@example.com"),
                                ["token"] = new OpenApiString("a1b2c3d4e5f6g7h8i9j0")
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
                            ["validationErrors"] = new OpenApiArray
                            {
                                new OpenApiString("Email cannot be empty"),
                                new OpenApiString("Please provide a valid email address")
                            }
                        }
                    }
                }
            };

            return operation;
        });
    }
}