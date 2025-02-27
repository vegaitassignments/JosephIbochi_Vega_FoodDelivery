using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Entities;

public class Food
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public string? ImageUrl { get; set; } = string.Empty;
    public string? Description { get; set; } = string.Empty;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<FoodRating> Ratings { get; set; } = new List<FoodRating>();
    public double AverageRating => Ratings.Any() ? Ratings.Average(r => r.Rating) : 0;
    public string Category { get; set; } = string.Empty; 
}