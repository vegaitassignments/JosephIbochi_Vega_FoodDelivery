using FoodDelivery.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Authentication.Feauture.ForgotPassword;

public class Command : IRequest<BaseResponse>
{
    public UpdatePasswordDTO requestData { get; set; }
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
        var user = await _userManager.FindByEmailAsync(request.requestData.Email);
        if (user == null)
        {
           return new BaseResponse {
                Message = "A reset token has been generated for you if email exist",
                Status = true
           };
        }

        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        return new BaseResponse { 
            Status = true, 
            Message = "Use this token to reset the password.", 
            Data = new { Email = user.Email, Token = token } 
        };
    }
}