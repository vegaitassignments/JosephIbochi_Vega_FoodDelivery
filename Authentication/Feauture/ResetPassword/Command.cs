
using FoodDelivery.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Authentication.Feauture.ResetPassword;

public class Command : IRequest<BaseResponse>
{
    public ResetPasswordDTO requestData { get; set; }
}

public class CommandHandler : IRequestHandler<Command, BaseResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    public CommandHandler(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    public async Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByEmailAsync(request.requestData.Email) ?? throw new FoodDeliveryBadRequestException("Invalid request");

        var result = await _userManager.ResetPasswordAsync(user, request.requestData.Token, request.requestData.NewPassword);
        if (!result.Succeeded) {
            throw new FoodDeliveryBadRequestException("Invalid token provided");
        }

        return new BaseResponse {
            Status = true,
            Message = "Password reset successful"
        };
    }
}
