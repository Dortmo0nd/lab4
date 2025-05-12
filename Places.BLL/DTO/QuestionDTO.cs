namespace Places.BLL.DTO
{
    public class QuestionDTO
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public int PlaceId { get; set; }
        public List<AnswerDTO> Answers { get; set; } = new List<AnswerDTO>();
    }
}