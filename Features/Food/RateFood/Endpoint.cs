
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Features.Food.RateFood;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/foods/{id}/rate", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (ISender sender, IValidator<RateFoodDTO> validator, int id, [FromBody] RateFoodDTO requestData) => {
                var validationResult = validator.Validate(requestData);
                if (validationResult.IsValid) {
                    return Results.Ok(await sender.Send(new Command{requestData = requestData, FoodId = id}));
                }

                return Results.BadRequest(new BaseResponse {
                    ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage),
                    Status = false,
                    Message = "Failed to add rating"
                });
            }
        )
        .WithName("RateFood")
        .WithTags("Food")
        .WithOpenApi(operation => new(operation) {
            Summary = "Rate food",
            OperationId = "RateFood",
            Description = "Allows a user to rate and comment on a food"
        });
    }
}

// 400 200 404