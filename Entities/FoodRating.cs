
using System.ComponentModel.DataAnnotations;

namespace FoodDelivery.Entities;

public class FoodRating
{
    [Key]
    public int Id { get; set; }
    public int FoodId { get; set; }
    public Food Food { get; set; }
    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; }
    public int Rating { get; set; }
    public string Comment { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}