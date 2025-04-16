namespace Places.BLL.DTO
{
    public class AnswerDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int QuestionId { get; set; }
        public int UserId { get; set; }
    }
}