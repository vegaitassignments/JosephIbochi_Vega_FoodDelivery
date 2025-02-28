using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace FoodDelivery.Entities;

public class ApplicationUser: IdentityUser<Guid>
{
    public string Name { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
    public ICollection<FoodRating> FoodRatings { get; set; } = new List<FoodRating>();
}