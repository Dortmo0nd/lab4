namespace Places.Models;

public class Review
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int PlaceId { get; set; }
    public virtual Place Place { get; set; } 
    public int UserId { get; set; }
    public virtual User User { get; set; } 
}