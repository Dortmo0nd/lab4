using Places.BLL.DTO;
using Places.Models;

namespace Places.BLL.Mappers
{
    public class AnswerMapper
    {
        public AnswerDTO ToDto(Answer answer)
        {
            if (answer == null) return null;
            return new AnswerDTO
            {
                Id = answer.Id,
                Content = answer.Content,
                QuestionId = answer.QuestionId,
                UserId = answer.UserId
            };
        }

        public Answer ToEntity(AnswerDTO answerDto)
        {
            if (answerDto == null) return null;
            return new Answer
            {
                Id = answerDto.Id,
                Content = answerDto.Content,
                QuestionId = answerDto.QuestionId,
                UserId = answerDto.UserId
            };
        }
    }
}