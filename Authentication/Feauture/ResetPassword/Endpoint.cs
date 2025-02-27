namespace FoodDelivery.Authentication.Feauture.ResetPassword;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/reset-password", 
            async (ISender sender, IValidator<ResetPasswordDTO> validator, ResetPasswordDTO requestData) => {
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
        .WithName("ResetPassword")
        .WithTags("Auth")
        .WithOpenApi(operation => new(operation) {
            Summary = "Reset password",
            OperationId = "ResetPassword",
            Description = "Allows a user reset passwor via a rest link"
        });
    }
}