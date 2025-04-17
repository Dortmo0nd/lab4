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
    public class AnswerService : IAnswerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly AnswerMapper _mapper;

        public AnswerService(IUnitOfWork unitOfWork, AnswerMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public AnswerDTO GetAnswerById(int id)
        {
            var answer = _unitOfWork.AnswerRepository.GetById(id);
            return _mapper.ToDto(answer);
        }

        public IEnumerable<AnswerDTO> GetAllAnswers()
        {
            var answers = _unitOfWork.AnswerRepository.GetAll();
            return answers.Select(a => _mapper.ToDto(a));
        }

        public void AddAnswer(AnswerDTO answerDto)
        {
            if (answerDto == null || string.IsNullOrEmpty(answerDto.Content) || answerDto.QuestionId <= 0 || answerDto.UserId <= 0)
                throw new ArgumentException("Invalid answer data");

            var answer = _mapper.ToEntity(answerDto);
            _unitOfWork.AnswerRepository.Add(answer);
            _unitOfWork.SaveChanges();
        }

        public void UpdateAnswer(AnswerDTO answerDto)
        {
            if (answerDto == null || string.IsNullOrEmpty(answerDto.Content) || answerDto.QuestionId <= 0 || answerDto.UserId <= 0)
                throw new ArgumentException("Invalid answer data");

            var answer = _mapper.ToEntity(answerDto);
            _unitOfWork.AnswerRepository.Update(answer);
            _unitOfWork.SaveChanges();
        }

        public void DeleteAnswer(int id)
        {
            var answer = _unitOfWork.AnswerRepository.GetById(id);
            if (answer != null)
            {
                _unitOfWork.AnswerRepository.Delete(answer);
                _unitOfWork.SaveChanges();
            }
        }
    }
}
