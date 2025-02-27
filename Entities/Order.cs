using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Entities;

public class Order
{
    [Key]
    public int Id { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
    public int RestaurantId { get; set; }
    public Restaurant Restaurant { get; set; }
    public Guid CourierId { get; set; }
    public decimal TotalPrice { get; set; }
    public Status Status { get; set; }   
     public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();

}

public enum Status {
    Pending,
    InProgress,
    Delivered,
    Cancelled
}