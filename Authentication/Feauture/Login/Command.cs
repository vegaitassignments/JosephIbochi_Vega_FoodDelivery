using FoodDelivery.Authentication.Helper;
using FoodDelivery.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Authentication.Feauture.Login;

public class Command : IRequest<BaseResponse>
{
    public LoginDTO requestData { get; set; }
}

public class CommandHandler : IRequestHandler<Command, BaseResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IConfiguration _config;
    public CommandHandler(UserManager<ApplicationUser> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }
    public async Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        var data = request.requestData;

        var user = await _userManager.FindByEmailAsync(data.Email) ?? throw new FoodDeliveryUnAuthorizedException("Invalid login credential");

        var isPasswordValid = await _userManager.CheckPasswordAsync(user, data.Password);
        if(!isPasswordValid) {
            throw new FoodDeliveryUnAuthorizedException("Invalid login credential");
        }

        var token = await GenerateToken.CreateToken(_config, user, _userManager, 2);

        return new BaseResponse {
            Data = new {token = token},
            Message = "Login successful",
            Status = true
        };
    }
}