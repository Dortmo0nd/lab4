namespace Places.Models;

public class Answer
{
    public int Id { get; set; }
    public string Content { get; set; }
    public int QuestionId { get; set; }
    public Question Question { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}