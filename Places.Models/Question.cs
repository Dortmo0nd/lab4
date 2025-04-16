using System.Collections.Generic;

namespace Places.Models;

public class Question
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int PlaceId { get; set; }
    public Place Place { get; set; }
    public ICollection<Answer> Answers { get; set; }
}