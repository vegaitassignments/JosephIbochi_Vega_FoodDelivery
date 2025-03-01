using FoodDelivery.Authentication.Feauture.Login;

namespace FoodDelivery.Authentication.Feature.Login;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/login", 
            async (ISender sender, IValidator<LoginDTO> validator, LoginDTO requestData) =>
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
        .WithName("Login")
        .WithTags("Auth")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Authenticate user and retrieve a JWT token";
            operation.OperationId = "Login";
            operation.Description = "Validates user credentials and returns an authentication token.";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "User successfully logged in",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["message"] = new OpenApiString("Login successful"),
                            ["data"] = new OpenApiObject
                            {
                                ["token"] = new OpenApiString("eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...")
                            }
                        }
                    }
                }
            };

            operation.Responses["400"] = new OpenApiResponse
            {
                Description = "Validation error - Invalid email or missing fields",
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
                                new OpenApiString("Password cannot be empty")
                            }
                        }
                    }
                }
            };

            operation.Responses["401"] = new OpenApiResponse
            {
                Description = "Invalid login credentials",
                Content =
                {
                    ["application/problem+json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["type"] = new OpenApiString("https://tools.ietf.org/html/rfc7231#section-6.5.1"),
                            ["title"] = new OpenApiString("Unauthorized"),
                            ["status"] = new OpenApiInteger(401),
                            ["detail"] = new OpenApiString("Invalid login credentials"),
                            ["instance"] = new OpenApiString("/auth/login")
                        }
                    }
                }
            };

            return operation;
        });
    }
}