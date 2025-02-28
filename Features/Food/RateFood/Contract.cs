namespace FoodDelivery.Features.Food.RateFood;

public record RateFoodDTO(string Comment, int Rating);

public class RateFoodDTOValidator : AbstractValidator<RateFoodDTO>
{
    public RateFoodDTOValidator()
    {
        RuleFor(x => x.Rating)
            .InclusiveBetween(1, 5).WithMessage("Rating must be between 1 and 5.");

        RuleFor(x => x.Comment)
            .MaximumLength(500).WithMessage("Comment cannot be longer than 500 characters.");
    }
}