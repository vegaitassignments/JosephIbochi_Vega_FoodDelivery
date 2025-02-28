
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace FoodDelivery.Features.Restaurant.CreateRestaurant;

public class Endpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost(
            "/restaurants", 
            [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Policy = "Admin")] 
            async (ISender sender, IValidator<CreateRestaurantDTO> validator, [FromBody] CreateRestaurantDTO requestData
        ) => {
                var validationResult = await validator.ValidateAsync(requestData);

                if (validationResult.IsValid){
                    return Results.Ok(await sender.Send(new Command {reequestData = requestData}));
                }

                return Results.BadRequest(new BaseResponse {
                    ValidationErrors = validationResult.Errors.Select(e => e.ErrorMessage),
                    Status = false,
                    Message = "Failed to create a restaurant"
                }); 
            })
            .WithName("CreateRestaurant")
            .WithTags("Restaurant")
            .WithOpenApi(operation => new(operation) {
                Summary = "Create restaurant",
                OperationId = "CreateRestaurant",
                Description = "Allows to remove a restaurant",
                Responses = {
                    ["200"] = new OpenApiResponse { Description = "Restaurant and courier created successfully."},
                    ["400"] = new OpenApiResponse { Description = "Invalid request data"},
                }
            });
    }
}