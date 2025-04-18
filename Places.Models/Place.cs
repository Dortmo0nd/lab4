namespace Places.Models;

public class Place
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public virtual ICollection<Review> Reviews { get; set; } 
    public virtual ICollection<Question> Questions { get; set; } 
    public virtual ICollection<Media> MediaFiles { get; set; } 
}