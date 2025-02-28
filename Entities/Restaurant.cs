
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDelivery.Entities;

public class Restaurant
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    // public string Location { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    [NotMapped]
    public bool IsAvailable => !IsLockedUntil.HasValue || IsLockedUntil.Value <= DateTime.UtcNow;
    public DateTime? IsLockedUntil { get; set; }
    public Courier Courier { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}