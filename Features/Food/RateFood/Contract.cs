namespace FoodDelivery.Features.Food.RateFood;

public class RateFoodDTO(
    string Comment,
    int Rating
);

public class RateFoodDTOValidator : AbstractValidator<RateFoodDTO>
{
    public RateFoodDTOValidator()
    {
        
    }
}