namespace FoodDelivery.Features.Food.AddFood;

public record CreateFoodDTO(
    string Name,
    string Category,
    decimal Price,
    string? ImageUrl,
    string? Description
);


public class CreateFoodDTOValidator:  AbstractValidator<CreateFoodDTO>
{
    public CreateFoodDTOValidator()
    {
        RuleFor(f => f.Name)
            .NotEmpty().WithMessage("Food name is required.")
            .MaximumLength(100).WithMessage("Food name cannot exceed 100 characters.");
         RuleFor(f => f.Category)
            .NotEmpty().WithMessage("Category is required.")
            .MaximumLength(50).WithMessage("Category cannot exceed 50 characters.");
        RuleFor(f => f.Price)
            .GreaterThan(0).WithMessage("Price must be greater than zero.");
        RuleFor(f => f.ImageUrl)
            .Must(uri => string.IsNullOrEmpty(uri) || Uri.TryCreate(uri, UriKind.Absolute, out _))
            .WithMessage("Invalid image URL format.");
        RuleFor(f => f.Description)
            .MaximumLength(500).WithMessage("Description cannot exceed 500 characters.");
    }
}