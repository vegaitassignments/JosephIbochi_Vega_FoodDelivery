using FoodDelivery.Authentication.Feauture.ResetPassword;

namespace FoodDelivery.Authentication.Feature.ResetPassword;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/reset-password",
            async (ISender sender, IValidator<ResetPasswordDTO> validator, ResetPasswordDTO requestData) =>
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
        .WithName("ResetPassword")
        .WithTags("Auth")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Reset Password";
            operation.OperationId = "ResetPassword";
            operation.Description = "Allows a user to reset their password using a reset link.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Password reset successful",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["message"] = new OpenApiString("Password reset successful")
                        }
                    }
                }
            };

            operation.Responses["400"] = new OpenApiResponse
            {
                Description = "Validation failed or invalid token provided",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(false),
                            ["message"] = new OpenApiString("Password reset failed"),
                            ["validationErrors"] = new OpenApiArray
                            {
                                new OpenApiString("Email cannot be empty"),
                                new OpenApiString("Password must be at least 8 characters long.")
                            }
                        }
                    }
                }
            };

            operation.Responses["400"] = new OpenApiResponse
            {
                Description = "Invalid token provided",
                Content =
                {
                    ["application/problem+json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["title"] = new OpenApiString("Bad Request"),
                            ["status"] = new OpenApiInteger(400),
                            ["detail"] = new OpenApiString("Invalid token provided."),
                        }
                    }
                }
            };

            return operation;
        });
    }
}