namespace FoodDelivery.Features.Food.GetAFood;

public record FoodDTO(
    int Id,
    string Name,
    decimal Price,
    string? ImageUrl,
    string? Description,
    double AverageRating,
    string Category,
    List<FoodRatingDTO> FoodRatings
);

public record FoodRatingDTO(
    int Rating,
    string Comment,
    DateTime CreatedAt
);