using System.Collections.Generic;
using Places.BLL.DTO;

namespace Places.BLL.Interfaces
{
    public interface IQuestionService
    {
        QuestionDTO GetQuestionById(int id);
        IEnumerable<QuestionDTO> GetAllQuestions();
        void AddQuestion(QuestionDTO question, int userId);
        void UpdateQuestion(QuestionDTO question);
        void DeleteQuestion(int id);
    }
}