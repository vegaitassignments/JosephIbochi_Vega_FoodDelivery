namespace FoodDelivery.Authentication.Feauture.ForgotPassword;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/forgot-password", 
            async (ISender sender, IValidator<UpdatePasswordDTO> validator, UpdatePasswordDTO requestData) => {
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
        .WithName("ForgotPassword")
        .WithTags("Auth")
        .WithOpenApi(operation => new(operation) {
            Summary = "Forgot password",
            OperationId = "ForgotPassword",
            Description = ""
        });
    }
}