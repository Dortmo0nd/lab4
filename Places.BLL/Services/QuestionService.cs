using System;
using System.Collections.Generic;
using System.Linq;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using Places.BLL.Mappers;
using Places.Abstract;
using Places.Models;

namespace Places.BLL.Services
{
    public class QuestionService : IQuestionService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly QuestionMapper _mapper;

        public QuestionService(IUnitOfWork unitOfWork, QuestionMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public QuestionDTO GetQuestionById(int id)
        {
            var question = _unitOfWork.QuestionRepository.GetById(id);
            return _mapper.ToDto(question);
        }

        public IEnumerable<QuestionDTO> GetAllQuestions()
        {
            var questions = _unitOfWork.QuestionRepository.GetAll();
            return questions.Select(q => _mapper.ToDto(q));
        }

        public void AddQuestion(QuestionDTO questionDto, int userId)
        {
            if (questionDto == null || string.IsNullOrEmpty(questionDto.Content) || questionDto.PlaceId <= 0)
                throw new ArgumentException("Invalid question data");

            var user = _unitOfWork.UserRepository.GetById(userId);
            if (user == null || user.Role != User.UserRole.Admin)
                throw new UnauthorizedAccessException("Only admins can add questions");

            var question = _mapper.ToEntity(questionDto);
            _unitOfWork.QuestionRepository.Add(question);
            _unitOfWork.SaveChanges();
        }

        public void UpdateQuestion(QuestionDTO questionDto)
        {
            if (questionDto == null || string.IsNullOrEmpty(questionDto.Content) || questionDto.PlaceId <= 0)
                throw new ArgumentException("Invalid question data");

            var question = _mapper.ToEntity(questionDto);
            _unitOfWork.QuestionRepository.Update(question);
            _unitOfWork.SaveChanges();
        }

        public void DeleteQuestion(int id)
        {
            var question = _unitOfWork.QuestionRepository.GetById(id);
            if (question != null)
            {
                _unitOfWork.QuestionRepository.Delete(question);
                _unitOfWork.SaveChanges();
            }
        }

        public QuestionDTO GetQuestionByContent(string content)
        {
            var question = _unitOfWork.QuestionRepository.Find(q => q.Content == content).FirstOrDefault();
            return _mapper.ToDto(question);
        }
        
        public QuestionDTO GetQuestionWithAnswersById(int id)
        {
            var question = _unitOfWork.QuestionRepository.GetWithInclude(q => q.Answers).FirstOrDefault(q => q.Id == id);
            return _mapper.ToDto(question);
        }
    }
}