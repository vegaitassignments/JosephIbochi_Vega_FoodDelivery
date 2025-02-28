namespace FoodDelivery.Features.Order.PlaceOrder;

public record PlaceOrderDTO(List<OrderItemsDTO> OrderItems);
public record OrderItemsDTO(int FoodId, int Quantity);

public record PlaceOrderResponseDTO(
    int OrderId,
    string CustomerName,
    decimal TotalPrice,
    string Status,
    DateTime CreatedAt,
    string RestaurantName
);