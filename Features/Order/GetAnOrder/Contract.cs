namespace FoodDelivery.Features.Order.GetAnOrder;

public record OrderDTO
(
    int Id,
    decimal TotalPrice,
    string Status,
    DateTime CreatedAt,
    int RestaurantId,
    string RestaurantName,
    List<OrderItemDTO> OrderItems
);

public record OrderItemDTO
(
    int FoodId,
    string FoodName,
    decimal ItemPrice,
    int Quantity
);