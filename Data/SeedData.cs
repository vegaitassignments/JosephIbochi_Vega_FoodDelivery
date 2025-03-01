using FoodDelivery.Entities;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Data;

public static class SeedData
{
    public static async Task SeedDataAsync(IServiceProvider serviceProvider, IConfiguration config)
    {
        using var scope = serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>(); 
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<Guid>>>();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        context.Database.EnsureCreated();

        if (!await roleManager.RoleExistsAsync("Admin"))
        {
            await roleManager.CreateAsync(new IdentityRole<Guid>("Admin"));
        }

        var email = config["AdminCredential:Email"];
        var password = config["AdminCredential:Password"];
        var adminUser = await userManager.FindByEmailAsync(email);
        string userName = config["AdminCredential:Email"];
        
        if (adminUser == null)
        {
            adminUser = new ApplicationUser {
                Email = email,
                UserName = userName,
                Name = "Super Admin"
            };

            var result = await userManager.CreateAsync(adminUser, password);
            if (result.Succeeded)
            {
                await userManager.AddToRoleAsync(adminUser, "Admin");
            }
            else {
                Console.WriteLine(result.Errors);
            }
        }

        await context.SaveChangesAsync();
    }
}