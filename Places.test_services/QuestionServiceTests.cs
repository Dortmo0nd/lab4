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
    public class QuestionServiceTests
    {
        private QuestionService _questionService;
        private IUnitOfWork _unitOfWork;
        private QuestionMapper _questionMapper;
        private Fixture _fixture;
        protected IKernel Kernel { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Kernel = new StandardKernel();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _questionMapper = new QuestionMapper();
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWork.QuestionRepository.Returns(Substitute.For<IRepository<Question>>());
            _unitOfWork.UserRepository.Returns(Substitute.For<IRepository<User>>());

            Rebind<IUnitOfWork>(_unitOfWork);
            Rebind<QuestionMapper>(_questionMapper);

            _questionService = new QuestionService(_unitOfWork, _questionMapper);
        }

        protected void Rebind<T>(T instance)
        {
            Kernel.Rebind<T>().ToConstant(instance);
        }

        [Test]
        public void GetQuestionById_ExistingId_ReturnsQuestionDto()
        {
            var question = _fixture.Build<Question>()
                .With(q => q.Id, 1)
                .With(q => q.Content, "Test question")
                .With(q => q.PlaceId, 1)
                .Create();
            _unitOfWork.QuestionRepository.GetById(1).Returns(question);

            var result = _questionService.GetQuestionById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Content, Is.EqualTo("Test question"));
            Assert.That(result.PlaceId, Is.EqualTo(1));
        }

        [Test]
        public void GetQuestionById_NonExistingId_ReturnsNull()
        {
            _unitOfWork.QuestionRepository.GetById(999).Returns((Question)null);

            var result = _questionService.GetQuestionById(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetAllQuestions_ReturnsAllQuestions()
        {
            var questions = _fixture.CreateMany<Question>(3).ToList();
            _unitOfWork.QuestionRepository.GetAll().Returns(questions);

            var result = _questionService.GetAllQuestions();

            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AddQuestion_ValidQuestionDtoAndAdminUser_AddsQuestion()
        {
            var questionDto = _fixture.Build<QuestionDTO>()
                .With(q => q.Id, 0)
                .With(q => q.Content, "New question")
                .With(q => q.PlaceId, 1)
                .Create();
            var adminUser = _fixture.Build<User>()
                .With(u => u.Id, 1)
                .With(u => u.Role, User.UserRole.Admin)
                .Create();
            _unitOfWork.UserRepository.GetById(1).Returns(adminUser);

            _questionService.AddQuestion(questionDto, 1);

            _unitOfWork.QuestionRepository.Received(1).Add(Arg.Is<Question>(q =>
                q.Content == "New question" &&
                q.PlaceId == 1
            ));
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void AddQuestion_NonAdminUser_ThrowsUnauthorizedAccessException()
        {
            var questionDto = _fixture.Build<QuestionDTO>()
                .With(q => q.Content, "New question")
                .With(q => q.PlaceId, 1)
                .Create();
            var visitorUser = _fixture.Build<User>()
                .With(u => u.Id, 2)
                .With(u => u.Role, User.UserRole.Visitor)
                .Create();
            _unitOfWork.UserRepository.GetById(2).Returns(visitorUser);

            Assert.Throws<UnauthorizedAccessException>(() => _questionService.AddQuestion(questionDto, 2));
        }

        [Test]
        public void AddQuestion_InvalidQuestionDto_ThrowsArgumentException()
        {
            var questionDto = _fixture.Build<QuestionDTO>()
                .With(q => q.Content, "")
                .With(q => q.PlaceId, 0)
                .Create();
            var adminUser = _fixture.Build<User>()
                .With(u => u.Id, 1)
                .With(u => u.Role, User.UserRole.Admin)
                .Create();
            _unitOfWork.UserRepository.GetById(1).Returns(adminUser);

            Assert.Throws<ArgumentException>(() => _questionService.AddQuestion(questionDto, 1));
        }

        [Test]
        public void UpdateQuestion_ValidQuestionDto_UpdatesQuestion()
        {
            var existingQuestion = _fixture.Build<Question>()
                .With(q => q.Id, 1)
                .Create();
            var questionDto = _fixture.Build<QuestionDTO>()
                .With(q => q.Id, 1)
                .With(q => q.Content, "Updated question")
                .With(q => q.PlaceId, 2)
                .Create();
            _unitOfWork.QuestionRepository.GetById(1).Returns(existingQuestion);

            _questionService.UpdateQuestion(questionDto);

            _unitOfWork.QuestionRepository.Received(1).Update(Arg.Is<Question>(q =>
                q.Id == 1 &&
                q.Content == "Updated question" &&
                q.PlaceId == 2
            ));
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void UpdateQuestion_InvalidQuestionDto_ThrowsArgumentException()
        {
            var questionDto = _fixture.Build<QuestionDTO>()
                .With(q => q.Content, "")
                .With(q => q.PlaceId, 0)
                .Create();

            Assert.Throws<ArgumentException>(() => _questionService.UpdateQuestion(questionDto));
        }

        [Test]
        public void DeleteQuestion_ExistingId_DeletesQuestion()
        {
            var question = _fixture.Build<Question>().With(q => q.Id, 1).Create();
            _unitOfWork.QuestionRepository.GetById(1).Returns(question);

            _questionService.DeleteQuestion(1);

            _unitOfWork.QuestionRepository.Received(1).Delete(question);
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void DeleteQuestion_NonExistingId_DoesNotCallDelete()
        {
            _unitOfWork.QuestionRepository.GetById(999).Returns((Question)null);

            _questionService.DeleteQuestion(999);

            _unitOfWork.QuestionRepository.DidNotReceive().Delete(Arg.Any<Question>());
            _unitOfWork.DidNotReceive().SaveChanges();
        }

        [Test]
        public void GetQuestionByContent_ExistingContent_ReturnsQuestionDto()
        {
            var question = _fixture.Build<Question>()
                .With(q => q.Content, "Test question")
                .Create();
            _unitOfWork.QuestionRepository.Find(Arg.Any<System.Linq.Expressions.Expression<Func<Question, bool>>>())
                .Returns(new List<Question> { question });

            var result = _questionService.GetQuestionByContent("Test question");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Content, Is.EqualTo("Test question"));
        }

        [Test]
        public void GetQuestionByContent_NonExistingContent_ReturnsNull()
        {
            _unitOfWork.QuestionRepository.Find(Arg.Any<System.Linq.Expressions.Expression<Func<Question, bool>>>())
                .Returns(new List<Question>());

            var result = _questionService.GetQuestionByContent("Non-existing question");

            Assert.That(result, Is.Null);
        }
    }
}