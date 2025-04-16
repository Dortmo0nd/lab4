using Places.BLL.DTO;
using Places.Models;

namespace Places.BLL.Mappers
{
    public class QuestionMapper
    {
        public QuestionDTO ToDto(Question question)
        {
            if (question == null) return null;
            return new QuestionDTO
            {
                Id = question.Id,
                Content = question.Content,
                PlaceId = question.PlaceId
            };
        }

        public Question ToEntity(QuestionDTO questionDto)
        {
            if (questionDto == null) return null;
            return new Question
            {
                Id = questionDto.Id,
                Content = questionDto.Content,
                PlaceId = questionDto.PlaceId
            };
        }
    }
}