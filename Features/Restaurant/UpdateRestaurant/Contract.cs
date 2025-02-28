namespace FoodDelivery.Features.Restaurant.UpdateRestaurant;

public record UpdateRestaurantDTO(
    string? Name,
    double? Latitude,
    double? Longitude,
    string? CourierName,
    string? CourierPhoneNumber
);

public class UpdateRestaurantValidator: AbstractValidator<UpdateRestaurantDTO>
{
    public UpdateRestaurantValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Restaurant Name cannot be empty").When(x => x.Name != null);
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).When(x => x.Latitude.HasValue)
            .WithMessage("Latitude must be between -90 and 90.");
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).When(x => x.Longitude.HasValue)
            .WithMessage("Longitude must be between -180 and 180.");
        RuleFor(x => x.CourierName).NotEmpty().WithMessage("Courier Name cannot be empty").When(x => x.Name != null);
        RuleFor(x => x.CourierPhoneNumber)
            .Matches(@"^\+?\d{10,15}$").When(x => !string.IsNullOrEmpty(x.CourierPhoneNumber))
            .WithMessage("Invalid phone number format.");
    }
}