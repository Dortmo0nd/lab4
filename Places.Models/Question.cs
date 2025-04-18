namespace Places.Models;

public class Question
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int PlaceId { get; set; }
    public virtual Place Place { get; set; } 
    public virtual ICollection<Answer> Answers { get; set; } 
}