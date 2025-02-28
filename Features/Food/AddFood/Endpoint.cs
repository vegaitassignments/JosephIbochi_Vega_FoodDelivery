
namespace FoodDelivery.Features.Food.AddFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/foods", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")] 
            async (ISender sender, IValidator<CreateFoodDTO> validator, CreateFoodDTO requestData) => {
                var validationResult = validator.Validate(requestData);
                if (validationResult.IsValid) {
                    return Results.Ok(await sender.Send(new Command{requestData = requestData}));
                }

                return Results.BadRequest(new BaseResponse {
                    ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage),
                    Status = false,
                    Message = "Failed to create food"
                });
            }
        )
        .WithName("AddFood")
        .WithTags("Food")
        .WithOpenApi(operation => new(operation) {
            Summary = "Add food",
            OperationId = "AddFood",
            Description = "Add a food to the menu \n\n Here is a sample request \n\n {\n\t\t 'name': 'Chicken', \n\t\t 'category': 'Drunstick', \n\t\t 'price': '123'}"
        });
    }
}