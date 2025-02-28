namespace FoodDelivery.Features.Restaurant.GetOrdersByRestaurant;

public record GetLessRestaurantOrdersDetailsDTO(
    int OrderId,
    string UserName,
    decimal TotalPrice,
    string Status,
    DateTime CreatedAt
);