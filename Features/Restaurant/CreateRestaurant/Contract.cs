namespace FoodDelivery.Features.Restaurant.CreateRestaurant;

public record CreateRestaurantDTO(
    string Name,
    double Latitude,
    double Longitude,
    CreateCourier Courier
);

public record CreateCourier(
    string Name,
    string PhoneNumber
);

public class CreateRestaurantValidator: AbstractValidator<CreateRestaurantDTO>
{
    public CreateRestaurantValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("Restaurant Name is required.");
        RuleFor(x => x.Latitude).InclusiveBetween(-90, 90).WithMessage("Latitude must be between -90 and 90.");
        RuleFor(x => x.Longitude).InclusiveBetween(-180, 180).WithMessage("Longitude must be between -180 and 180.");
        RuleFor(x => x.Courier.Name).NotNull().NotEmpty().WithMessage("Courier name is required.");
        RuleFor(x => x.Courier.PhoneNumber)
            .NotEmpty().WithMessage("Phone number is required.")
            .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number format.");
    }
}

// public class CourierValidator : AbstractValidator<CreateCourier>
// {
//     public CourierValidator()
//     {
//         RuleFor(x => x.Name).NotEmpty().WithMessage("Courier name is required.");
//         RuleFor(x => x.PhoneNumber)
//             .NotEmpty().WithMessage("Phone number is required.")
//             .Matches(@"^\+?\d{10,15}$").WithMessage("Invalid phone number format.");
//     }
// }