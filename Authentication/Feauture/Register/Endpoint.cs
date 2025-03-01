using FoodDelivery.Authentication.Feauture.Register;

namespace FoodDelivery.Authentication.Feature.Register;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/register", 
            async (ISender sender, IValidator<RegisterDTO> validator, RegisterDTO requestData) =>
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
        .WithName("Register")
        .WithTags("Auth")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Register a new user";
            operation.OperationId = "Register";
            operation.Description = "Registers a new user and determines their precise location for optimized delivery.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "User successfully registered",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["message"] = new OpenApiString("Registration successful")
                        }
                    }
                }
            };

            operation.Responses["400"] = new OpenApiResponse
            {
                Description = "Validation error or registration failure",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(false),
                            ["message"] = new OpenApiString("User registration failed"),
                            ["validationErrors"] = new OpenApiArray
                            {
                                new OpenApiString("Email is required."),
                                new OpenApiString("Password must be at least 8 characters long.")
                            }
                        }
                    }
                }
            };

            operation.Responses["403"] = new OpenApiResponse
            {
                Description = "Unauthorized to create an admin account",
                Content =
                {
                    ["application/problem+json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["title"] = new OpenApiString("Forbidden"),
                            ["status"] = new OpenApiInteger(403),
                            ["detail"] = new OpenApiString("You are not authorized to create an admin."),
                        }
                    }
                }
            };

            return operation;
        });
    }
}