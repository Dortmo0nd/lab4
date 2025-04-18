namespace Places.Models;

public class Media
{
    public int Id { get; set; }
    public string FilePath { get; set; }
    public string Type { get; set; }
    public int? PlaceId { get; set; }
    public virtual Place Place { get; set; } 
    public int? UserId { get; set; }
    public virtual User User { get; set; } 
}