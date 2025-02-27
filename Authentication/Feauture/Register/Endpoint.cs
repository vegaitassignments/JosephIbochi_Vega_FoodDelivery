namespace FoodDelivery.Authentication.Feauture.Register;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/auth/register", 
            async (ISender sender, IValidator<RegisterDTO> validator, RegisterDTO requestData) => {
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
        .WithName("Register")
        .WithTags("Auth")
        .WithOpenApi(operation => new(operation) {
            Summary = "Register",
            OperationId = "Register",
            Description = "Registers a new user. Latitude and Longitude is used to determine the precise location of the user, to enable rerouting delivery from the closest restaurant to that user"
        });
    }
}