namespace FoodDelivery.Features.Food.UpdateFood;

public record UpdateFoodDTO(
    string? Name,
    string? Category,
    decimal? Price,
    string? ImageUrl,
    string? Description
);

public class UpdateFoodDTOValidator : AbstractValidator<UpdateFoodDTO>
{
    public UpdateFoodDTOValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .When(x => x.Name is not null)
            .WithMessage("Food name cannot be empty.");

        RuleFor(x => x.Category)
            .NotEmpty()
            .When(x => x.Category is not null)
            .WithMessage("Category cannot be empty.");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .When(x => x.Price.HasValue)
            .WithMessage("Price must be greater than 0.");

        RuleFor(x => x.ImageUrl)
            .Must(url => Uri.TryCreate(url, UriKind.Absolute, out _))
            .When(x => !string.IsNullOrEmpty(x.ImageUrl))
            .WithMessage("Invalid Image URL.");

        RuleFor(x => x.Description)
            .MaximumLength(500)
            .When(x => x.Description is not null)
            .WithMessage("Description must not exceed 500 characters.");
    }
}