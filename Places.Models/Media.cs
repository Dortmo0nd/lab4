namespace Places.Models;

public class Media
{
    public int Id { get; set; }
    public string FilePath { get; set; }
    public string Type { get; set; } // photo, video, etc.
    public int? PlaceId { get; set; }
    public Place Place { get; set; }
    public int? UserId { get; set; }
    public User User { get; set; }
}