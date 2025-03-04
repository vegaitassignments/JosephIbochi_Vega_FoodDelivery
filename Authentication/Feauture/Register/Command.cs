
using FoodDelivery.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Authentication.Feauture.Register;

public class Command : IRequest<BaseResponse>
{
    public RegisterDTO requestData { get; set; }
}

public class CommandHandler : IRequestHandler<Command, BaseResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;

    public CommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        var data = request.requestData;
        var role = "User";
        
        var existingUser = await _userManager.FindByEmailAsync(data.Email);
        if (existingUser != null) {
            throw new FoodDeliveryBadRequestException("User with email already exist");
        }

        var user = new ApplicationUser {
            Email = data.Email,
            Name = data.Name,
            Latitude = data.Latitude,
            Longitude = data.Longitude,
            EmailConfirmed = true,
            UserName = data.Email,
        };
        

        var result = await _userManager.CreateAsync(user, data.Password);
        if (!result.Succeeded)
        {
            throw new FoodDeliveryBadRequestException("User registeration failed");
        }
    

        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        await _userManager.AddToRoleAsync(user, role);

        return new BaseResponse {
            Message = "Registeration successful",
            Status = true
        };
    }
}
