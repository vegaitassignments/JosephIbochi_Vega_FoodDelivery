namespace FoodDelivery.Authentication.Feauture.Login;

public record LoginDTO(
    string Email,
    string Password
);

public class LoginDTOValidator : AbstractValidator<LoginDTO>
{
    public LoginDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Please provide a valid email address");
        RuleFor(x => x.Password)
            .NotEmpty().WithMessage("Password is required");
    }
}