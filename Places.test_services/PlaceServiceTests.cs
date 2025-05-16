using System.Linq.Expressions;
using NSubstitute;
using NUnit.Framework;
using AutoFixture;
using Ninject;
using Places.Abstract;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using Places.BLL.Mappers;
using Places.BLL.Services;
using Places.Models;
using Assert = NUnit.Framework.Assert;

namespace Places.test_services
{
    [TestFixture]
    public class PlaceServiceTests : PlaceIntegrationTests
    {
        private IPlaceService _placeService;
        private IUnitOfWork _unitOfWork;
        private PlaceMapper _placeMapper;
        private Fixture _fixture;
        protected IKernel Kernel { get; private set; } // Додано для Ninject

        [SetUp]
        public new void SetUp()
        {
            base.Setup();
            Kernel = new StandardKernel(); // Ініціалізація Ninject Kernel
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _placeMapper = new PlaceMapper();
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Налаштування моків репозиторіїв через Returns
            _unitOfWork.PlaceRepository.Returns(Substitute.For<IRepository<Place>>());
            _unitOfWork.ReviewRepository.Returns(Substitute.For<IRepository<Review>>());
            _unitOfWork.QuestionRepository.Returns(Substitute.For<IRepository<Question>>());
            _unitOfWork.MediaRepository.Returns(Substitute.For<IRepository<Media>>());

            Rebind<IUnitOfWork>(_unitOfWork);
            Rebind<PlaceMapper>(_placeMapper);

            _placeService = new PlaceService(_unitOfWork, _placeMapper);
        }

        // Метод для переприв’язки в Ninject
        protected void Rebind<T>(T instance)
        {
            Kernel.Rebind<T>().ToConstant(instance);
        }

        [Test]
        public void GetPlaceById_ExistingId_ReturnsPlaceDto()
        {
            var place = _fixture.Build<Place>()
                .With(p => p.Id, 1)
                .With(p => p.Reviews, new List<Review>())
                .With(p => p.Questions, new List<Question>())
                .With(p => p.MediaFiles, new List<Media>())
                .Create();
            _unitOfWork.PlaceRepository.Find(Arg.Any<Expression<Func<Place, bool>>>()).Returns(new List<Place> { place });
            _unitOfWork.PlaceRepository.GetWithInclude(
                Arg.Any<Expression<Func<Place, object>>[]>()).Returns(new List<Place> { place });

            var result = _placeService.GetPlaceById(1);

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
        }

        [Test]
        public void GetPlaceById_NonExistingId_ReturnsNull()
        {
            _unitOfWork.PlaceRepository.Find(Arg.Any<Expression<Func<Place, bool>>>()).Returns(new List<Place>());

            var result = _placeService.GetPlaceById(999);

            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetAllPlaces_ReturnsAllPlaces()
        {
            var places = _fixture.CreateMany<Place>(3).ToList();
            _unitOfWork.PlaceRepository.GetAll().Returns(places);

            var result = _placeService.GetAllPlaces();

            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AddPlace_ValidPlaceDto_AddsPlace()
        {
            var placeDto = _fixture.Build<PlaceDTO>()
                .With(p => p.Id, 0)
                .With(p => p.Name, "Test Place")
                .With(p => p.Description, "Description")
                .Create();

            _placeService.AddPlace(placeDto);

            _unitOfWork.PlaceRepository.Received(1).Add(Arg.Any<Place>());
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void AddPlace_InvalidPlaceDto_ThrowsArgumentException()
        {
            var placeDto = _fixture.Build<PlaceDTO>()
                .With(p => p.Name, "")
                .Create();

            Assert.Throws<ArgumentException>(() => _placeService.AddPlace(placeDto));
        }

        [Test]
        public void UpdatePlace_ValidPlaceDto_UpdatesPlace()
        {
            var place = _fixture.Build<Place>().With(p => p.Id, 1).Create();
            var placeDto = _fixture.Build<PlaceDTO>()
                .With(p => p.Id, 1)
                .With(p => p.Name, "Updated Place")
                .Create();
            _unitOfWork.PlaceRepository.GetById(1).Returns(place);

            _placeService.UpdatePlace(placeDto);

            _unitOfWork.PlaceRepository.Received(1).Update(Arg.Any<Place>());
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void UpdatePlace_InvalidPlaceDto_ThrowsArgumentException()
        {
            var placeDto = _fixture.Build<PlaceDTO>()
                .With(p => p.Name, "")
                .Create();

            Assert.Throws<ArgumentException>(() => _placeService.UpdatePlace(placeDto));
        }

        [Test]
        public void DeletePlace_ExistingId_DeletesPlace()
        {
            var place = _fixture.Build<Place>().With(p => p.Id, 1).Create();
            _unitOfWork.PlaceRepository.GetById(1).Returns(place);

            _placeService.DeletePlace(1);

            _unitOfWork.PlaceRepository.Received(1).Delete(place);
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void GetPlaceByName_ExistingName_ReturnsPlaceDto()
        {
            var place = _fixture.Build<Place>()
                .With(p => p.Name, "Test Place")
                .Create();
            _unitOfWork.PlaceRepository.Find(Arg.Any<Expression<Func<Place, bool>>>()).Returns(new List<Place> { place });

            var result = _placeService.GetPlaceByName("Test Place");

            Assert.That(result, Is.Not.Null);
            Assert.That(result.Name, Is.EqualTo("Test Place"));
        }
    }
}