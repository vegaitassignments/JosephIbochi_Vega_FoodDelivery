
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Entities;

public class Restaurant
{
    [Key]
    public int Id { get; set; }
    public string Name { get; set; }
    // public string Location { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public bool isEngaged { get; set; }
    public Courier Courier { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}