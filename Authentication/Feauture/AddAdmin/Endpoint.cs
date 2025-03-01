using FoodDelivery.Authentication.Feauture.AddAdmin;

namespace FoodDelivery.Authentication.Feature.AddAdmin;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/add-admin", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")]
            async (ISender sender, IValidator<AddAdminDTO> validator, AddAdminDTO requestData) =>
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
        .WithName("AddAdmin")
        .WithTags("Auth")
        .WithOpenApi(operation =>
        {
            operation.Summary = "Add an admin";
            operation.OperationId = "AddAdmin";

            operation.Responses["200"] = new OpenApiResponse
            {
                Description = "Create admin",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(true),
                            ["message"] = new OpenApiString("Admin successfully created")
                        }
                    }
                }
            };

            operation.Responses["400"] = new OpenApiResponse
            {
                Description = "Validation error or add failure",
                Content =
                {
                    ["application/json"] = new OpenApiMediaType
                    {
                        Example = new OpenApiObject
                        {
                            ["status"] = new OpenApiBoolean(false),
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