namespace FoodDelivery.Features.Restaurant.GetARestaurant;

public record GetRestaurantDTO(
    string Name,
    double Latitude,
    double Longitude,
    string CourierName,
    string CourierPhoneNumber,
    bool isEngaged
);