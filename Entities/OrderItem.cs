using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Entities;

public class OrderItem
{
    [Key]
    public int OrderId { get; set; }
    public Order Order { get; set; }
    public int FoodId { get; set; }
    public Food Food { get; set; }
    public int Quantity { get; set; }
    public decimal ItemPrice { get; set; }
}