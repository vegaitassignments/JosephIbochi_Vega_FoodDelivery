namespace FoodDelivery.Authentication.Feauture.ForgotPassword;

public record UpdatePasswordDTO(
    string Email
);

public class UpdatePasswordDTOValidator : AbstractValidator<UpdatePasswordDTO>
{
    public UpdatePasswordDTOValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty().WithMessage("Email cannot be empty")
            .EmailAddress().WithMessage("Please provide a valid email address");
    }
}