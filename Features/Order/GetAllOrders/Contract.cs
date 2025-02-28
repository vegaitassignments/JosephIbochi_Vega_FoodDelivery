namespace FoodDelivery.Features.Order.GetAllOrders;

public record GetLessRestaurantOrdersDetailsDTO(
    int OrderId,
    string UserName,
    decimal TotalPrice,
    string Status,
    DateTime CreatedAt,
    string RestaurantName
);