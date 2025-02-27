
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Features.Food.UpdateFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/foods/{id}", 
            // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (ISender sender, IValidator<UpdateFoodDTO> validator, int id, [FromBody] UpdateFoodDTO requestData) => {
                var validationResult = await validator.ValidateAsync(requestData);

                if (validationResult.IsValid) {
                    return Results.Ok(await sender.Send(new Command {FoodId = id, RequestData = requestData}));
                }

                return Results.BadRequest(new BaseResponse {
                    ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage),
                    Status = false,
                    Message = "Failed to update food"
                });
            }
        )
        .WithName("UpdateFood")
        .WithTags("Food")
        .WithOpenApi(operation => new(operation) {
            Summary = "Edit food",
            OperationId = "UpdateFood",
            Description = "Make changes to a food on the menu"
        });
    }
}