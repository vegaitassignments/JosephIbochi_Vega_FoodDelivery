using FoodDelivery.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
// using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace FoodDelivery.Data;

public class ApplicationDbContext: IdentityDbContext<ApplicationUser, IdentityRole<Guid>, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options){}

    public DbSet<Restaurant> Restaurants { get; set; }
    public DbSet<Courier> Couriers { get; set; }
    public DbSet<Food> Foods { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    public DbSet<FoodRating> FoodRatings { get; set; }


    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<Order>()
            .HasOne(o => o.User)
            .WithMany(u => u.Orders)
            .HasForeignKey(k => k.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<FoodRating>()
            .HasOne(f => f.User)
            .WithMany(u => u.FoodRatings)
            .HasForeignKey(k => k.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<Order>()
            .HasOne(r => r.Restaurant)
            .WithMany(o => o.Orders)
            .HasForeignKey(k => k.RestaurantId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Restaurant>()
            .HasOne(r => r.Courier)
            .WithOne(c => c.Restaurant)
            .HasForeignKey<Courier>(c => c.RestaurantId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<OrderItem>()
            .HasOne(oi => oi.Food)
            .WithMany(f => f.OrderItems)
            .HasForeignKey(oi => oi.FoodId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<FoodRating>()
            .HasOne(fr => fr.Food)
            .WithMany(f => f.Ratings)
            .HasForeignKey(fr => fr.FoodId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}