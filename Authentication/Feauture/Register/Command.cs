
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
    // private readonly IConfiguration _config;
    private readonly IHttpContextAccessor _httpContextAccessor;
    // private readonly ILogger _logger;

    public CommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole<Guid>> roleManager,  IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        // _config = config;
        _httpContextAccessor = httpContextAccessor;
        // _logger = logger;
    }

    public async Task<BaseResponse> Handle(Command request, CancellationToken cancellationToken)
    {
        var data = request.requestData;
        
        var existingUser = await _userManager.FindByEmailAsync(data.Email);
        if (existingUser != null) {
            throw new FoodDeliveryBadRequestException("User with email already exist");
        }

        string role = !data.isAdmin ? "User" : "Admin";
        if (role == "Admin")
        {
            var httpContext = _httpContextAccessor.HttpContext;
            var currentUser = httpContext?.User;

            if (currentUser == null || !currentUser.IsInRole("Admin"))
            {
                throw new FoodDeliveryForbiddenException("You are not authorized to create an admin");
            }
            // _logger.LogWarning($"Admin account created: {data.Email}");
        }
        
        var user = new ApplicationUser {
            Email = data.Email,
            Name = data.Name,
            Latitude = data.Latitude ?? null,
            Longitude = data.Longitude ?? null,
            EmailConfirmed = true,
            UserName = data.Email,
        };

        try {
            var result = await _userManager.CreateAsync(user, data.Password);
        } catch (Exception e) {
            throw new FoodDeliveryBadRequestException("User registeration failed");
        }
        // if (!result.Succeeded) {
        // }

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
