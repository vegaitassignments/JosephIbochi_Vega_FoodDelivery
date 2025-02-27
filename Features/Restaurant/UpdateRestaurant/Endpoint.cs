
using Microsoft.AspNetCore.Mvc;

namespace FoodDelivery.Features.Restaurant.UpdateRestaurant;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPut(
            "/restaurants/{id}", 
            // [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)] 
            async (ISender sender, IValidator<UpdateRestaurantDTO> validator, int id, [FromBody] UpdateRestaurantDTO requestData) => {
                var validationResult = await validator.ValidateAsync(requestData);
                if (validationResult.IsValid) {
                    return Results.Ok(await sender.Send(new Command{requestData = requestData, restaurantId = id}));
                }

                return Results.BadRequest(new BaseResponse {
                    ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage),
                    Status = false,
                    Message = "Failed to update restaurant"
                });
            }
        )
        .WithName("UpdateRestaurant")
        .WithTags("Restaurant")
        .WithOpenApi(operation => new(operation) {
            Summary = "Update restaurant",
            OperationId = "UpdateRestaurant",
            Description = "Allows update to be made to a restaurant"
        });
    }
}

// 404 400 200