using System.Collections.Generic;
using Places.BLL.DTO;

namespace Places.BLL.Interfaces
{
    public interface IAnswerService
    {
        AnswerDTO GetAnswerById(int id);
        IEnumerable<AnswerDTO> GetAllAnswers();
        void AddAnswer(AnswerDTO answer);
        void UpdateAnswer(AnswerDTO answer);
        void DeleteAnswer(int id);
    }
}