using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Entities;

public class Courier
{
    [Key]
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string PhoneNumber { get; set; }
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
}