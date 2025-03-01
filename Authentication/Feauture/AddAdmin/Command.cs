
using FoodDelivery.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Authentication.Feauture.AddAdmin;

public class Command : IRequest<BaseResponse>
{
    public AddAdminDTO requestData { get; set; }
}

public class CommandHandler : IRequestHandler<Command, BaseResponse>
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole<Guid>> _roleManager;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager,  IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        var data = request.requestData;
        var role = "Admin";
        
        var existingUser = await _userManager.FindByEmailAsync(data.Email);
        if (existingUser != null) {
            throw new FoodDeliveryBadRequestException("User with email already exist");
        }

        if (role == "Admin")
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var currentUser = httpContext?.User;

            if (currentUser == null || !currentUser.IsInRole("Admin"))
            {
                throw new FoodDeliveryForbiddenException("You are not authorized to create an admin");
            }
        }
        
        var user = new ApplicationUser {
            Email = data.Email,
            Name = data.Name,
            UserName = data.Email,
        };

        var result = await _userManager.CreateAsync(user, data.Password);

        if (!result.Succeeded) 
        {
            throw new FoodDeliveryBadRequestException("Failed to create an Admin");
        }

        if (!await _roleManager.RoleExistsAsync(role))
        {
            await _roleManager.CreateAsync(new IdentityRole<Guid>(role));
        }

        await _userManager.AddToRoleAsync(user, role);

        return new BaseResponse {
            Message = "Admin successfully created",
            Status = true
        };
    }
}
