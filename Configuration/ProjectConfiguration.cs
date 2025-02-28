
using System.Reflection;
using System.Text;
using FoodDelivery.Data;
using FoodDelivery.Entities;
using FoodDelivery.Middleware.GlobalExceptionMiddleware;
using Hangfire;
using Hangfire.MySql;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FoodDelivery.Configuration;

public static class ProjectConfiguration
{
    public static IServiceCollection AddProjectConfiguration(this IServiceCollection services, IConfiguration config, WebApplicationBuilder builder)
    {
        var assembly = Assembly.GetExecutingAssembly();
        var jwtSettings = config.GetSection("JwtSettings");
        var secretKey =  jwtSettings.GetValue<string>("Key");
        var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ?? config.GetConnectionString("DefaultConnection");
        
        services.AddHttpContextAccessor();

        services.AddDbContext<ApplicationDbContext>(options => {
            options.UseMySql(
                connectionString,
                ServerVersion.AutoDetect(connectionString),
                mySqlOptions => {
                    mySqlOptions.EnableRetryOnFailure();
                }
            );
        });

        services.AddHangfire(config =>
        config.UseStorage(new MySqlStorage(
            connectionString,
            new MySqlStorageOptions
            {
                QueuePollInterval = TimeSpan.FromSeconds(15), // Adjust based on your needs
                TablesPrefix = "Hangfire_" // Optional prefix for tables
        })));
        services.AddHangfireServer();
        
          
        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(assembly));
        services.AddCarter();
        services.AddValidatorsFromAssembly(assembly);
        services.AddEndpointsApiExplorer();
        
        // Custom services
        services.AddTransient<GlobalExceptionMiddleware>();

        services.AddAuthentication(options => {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options => {
            options.RequireHttpsMetadata = false;
            options.SaveToken = false;
            options.TokenValidationParameters = new TokenValidationParameters {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(secretKey ?? string.Empty)),
                ValidateLifetime = true,
                ValidateIssuer = false,
                ValidateAudience = false,
                ClockSkew = TimeSpan.Zero
            };
        });

        services.AddAuthorization(options => options.AddPolicy("Admin", policy => policy.RequireRole("Admin")));

        services.AddIdentity<ApplicationUser, IdentityRole<Guid>>(options => {
            // options.User.RequireUniqueEmail = true;
        })
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddDefaultTokenProviders();

        services.AddCors(options => {
            options.AddPolicy("AllowAny", builder => {
                builder.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });
        });
        return services;
    }
}