using System;
using System.Collections.Generic;
using System.Linq;
using NSubstitute;
using NUnit.Framework;
using AutoFixture;
using Ninject;
using Places.Abstract;
using Places.BLL.DTO;
using Places.BLL.Mappers;
using Places.BLL.Services;
using Places.Models;
using Assert = NUnit.Framework.Assert;

namespace Places.test_services
{
    [TestFixture]
    public class AnswerServiceTests
    {
        private AnswerService _answerService;
        private IUnitOfWork _unitOfWork;
        private AnswerMapper _answerMapper;
        private Fixture _fixture;
        protected IKernel Kernel { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Kernel = new StandardKernel();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _answerMapper = new AnswerMapper();
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWork.AnswerRepository.Returns(Substitute.For<IRepository<Answer>>());

            Rebind<IUnitOfWork>(_unitOfWork);
            Rebind<AnswerMapper>(_answerMapper);

            _answerService = new AnswerService(_unitOfWork, _answerMapper);
        }

        protected void Rebind<T>(T instance)
        {
            Kernel.Rebind<T>().ToConstant(instance);
        }

        [Test]
        public void GetAnswerById_ExistingId_ReturnsAnswerDto()
        {
            // Arrange
            var answer = _fixture.Build<Answer>()
                .With(a => a.Id, 1)
                .With(a => a.Content, "Test answer")
                .With(a => a.QuestionId, 1)
                .With(a => a.UserId, 1)
                .Create();
            _unitOfWork.AnswerRepository.GetById(1).Returns(answer);

            // Act
            var result = _answerService.GetAnswerById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Content, Is.EqualTo("Test answer"));
            Assert.That(result.QuestionId, Is.EqualTo(1));
            Assert.That(result.UserId, Is.EqualTo(1));
        }

        [Test]
        public void GetAnswerById_NonExistingId_ReturnsNull()
        {
            // Arrange
            _unitOfWork.AnswerRepository.GetById(999).Returns((Answer)null);

            // Act
            var result = _answerService.GetAnswerById(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetAllAnswers_ReturnsAllAnswers()
        {
            // Arrange
            var answers = _fixture.CreateMany<Answer>(3).ToList();
            _unitOfWork.AnswerRepository.GetAll().Returns(answers);

            // Act
            var result = _answerService.GetAllAnswers();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AddAnswer_ValidAnswerDto_AddsAnswer()
        {
            // Arrange
            var answerDto = _fixture.Build<AnswerDTO>()
                .With(a => a.Id, 0)
                .With(a => a.Content, "New answer")
                .With(a => a.QuestionId, 1)
                .With(a => a.UserId, 1)
                .Create();

            // Act
            _answerService.AddAnswer(answerDto);

            // Assert
            _unitOfWork.AnswerRepository.Received(1).Add(Arg.Is<Answer>(a =>
                a.Content == "New answer" &&
                a.QuestionId == 1 &&
                a.UserId == 1
            ));
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void AddAnswer_InvalidAnswerDto_ThrowsArgumentException()
        {
            // Arrange
            var answerDto = _fixture.Build<AnswerDTO>()
                .With(a => a.Content, "")
                .With(a => a.QuestionId, 0)
                .With(a => a.UserId, 0)
                .Create();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _answerService.AddAnswer(answerDto));
        }

        [Test]
        public void UpdateAnswer_ValidAnswerDto_UpdatesAnswer()
        {
            // Arrange
            var existingAnswer = _fixture.Build<Answer>()
                .With(a => a.Id, 1)
                .Create();
            var answerDto = _fixture.Build<AnswerDTO>()
                .With(a => a.Id, 1)
                .With(a => a.Content, "Updated answer")
                .With(a => a.QuestionId, 1)
                .With(a => a.UserId, 1)
                .Create();
            _unitOfWork.AnswerRepository.GetById(1).Returns(existingAnswer);

            // Act
            _answerService.UpdateAnswer(answerDto);

            // Assert
            _unitOfWork.AnswerRepository.Received(1).Update(Arg.Is<Answer>(a =>
                a.Id == 1 &&
                a.Content == "Updated answer" &&
                a.QuestionId == 1 &&
                a.UserId == 1
            ));
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void UpdateAnswer_InvalidAnswerDto_ThrowsArgumentException()
        {
            // Arrange
            var answerDto = _fixture.Build<AnswerDTO>()
                .With(a => a.Content, "")
                .With(a => a.QuestionId, 0)
                .With(a => a.UserId, 0)
                .Create();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _answerService.UpdateAnswer(answerDto));
        }

        [Test]
        public void DeleteAnswer_ExistingId_DeletesAnswer()
        {
            // Arrange
            var answer = _fixture.Build<Answer>().With(a => a.Id, 1).Create();
            _unitOfWork.AnswerRepository.GetById(1).Returns(answer);

            // Act
            _answerService.DeleteAnswer(1);

            // Assert
            _unitOfWork.AnswerRepository.Received(1).Delete(answer);
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void DeleteAnswer_NonExistingId_DoesNotCallDelete()
        {
            // Arrange
            _unitOfWork.AnswerRepository.GetById(999).Returns((Answer)null);

            // Act
            _answerService.DeleteAnswer(999);

            // Assert
            _unitOfWork.AnswerRepository.DidNotReceive().Delete(Arg.Any<Answer>());
            _unitOfWork.DidNotReceive().SaveChanges();
        }
    }
}