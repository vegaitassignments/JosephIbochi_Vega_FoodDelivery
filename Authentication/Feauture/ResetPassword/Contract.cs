namespace FoodDelivery.Authentication.Feauture.ResetPassword;

public record ResetPasswordDTO(
    string Email,
    string Token
);

public class ResetPasswordDTOValidator : AbstractValidator<ResetPasswordDTO>
{
    public ResetPasswordDTOValidator()
    {

    }
}