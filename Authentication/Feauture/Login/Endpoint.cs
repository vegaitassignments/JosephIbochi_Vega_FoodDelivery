namespace FoodDelivery.Authentication.Feauture.Login;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/login", 
            async (ISender sender, IValidator<LoginDTO> validator, LoginDTO requestData) => {
                var validationResult = validator.Validate(requestData);
                if (validationResult.IsValid) {
                    return Results.Ok(await sender.Send(new Command{requestData = requestData}));
                }

                return Results.BadRequest(new BaseResponse {
                    ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage),
                    Status = false,
                });
            }
        )
        .WithName("Login")
        .WithTags("Auth")
        .WithOpenApi(operation => new(operation) {
            Summary = "Login",
            OperationId = "Login",
            Description = ""
        });
    }
}