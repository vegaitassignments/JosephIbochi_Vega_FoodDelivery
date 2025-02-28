namespace FoodDelivery.Features.Restaurant.GetARestaurant;

public record GetRestaurantDTO(
    int Id,
    string Name,
    double Latitude,
    double Longitude,
    string CourierName,
    string CourierPhoneNumber,
    bool IsAvailable
);