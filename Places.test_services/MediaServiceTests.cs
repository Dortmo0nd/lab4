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
    public class MediaServiceTests
    {
        private MediaService _mediaService;
        private IUnitOfWork _unitOfWork;
        private MediaMapper _mediaMapper;
        private Fixture _fixture;
        protected IKernel Kernel { get; private set; }

        [SetUp]
        public void SetUp()
        {
            Kernel = new StandardKernel();
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _mediaMapper = new MediaMapper();
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWork.MediaRepository.Returns(Substitute.For<IRepository<Media>>());

            Rebind<IUnitOfWork>(_unitOfWork);
            Rebind<MediaMapper>(_mediaMapper);

            _mediaService = new MediaService(_unitOfWork, _mediaMapper);
        }

        protected void Rebind<T>(T instance)
        {
            Kernel.Rebind<T>().ToConstant(instance);
        }

        [Test]
        public void GetMediaById_ExistingId_ReturnsMediaDto()
        {
            // Arrange
            var media = _fixture.Build<Media>()
                .With(m => m.Id, 1)
                .With(m => m.FilePath, "path/to/file.jpg")
                .With(m => m.Type, "Photo")
                .With(m => m.PlaceId, 1)
                .With(m => m.UserId, 1)
                .Create();
            _unitOfWork.MediaRepository.GetById(1).Returns(media);

            // Act
            var result = _mediaService.GetMediaById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.FilePath, Is.EqualTo("path/to/file.jpg"));
            Assert.That(result.Type, Is.EqualTo("Photo"));
            Assert.That(result.PlaceId, Is.EqualTo(1));
            Assert.That(result.UserId, Is.EqualTo(1));
        }

        [Test]
        public void GetMediaById_NonExistingId_ReturnsNull()
        {
            // Arrange
            _unitOfWork.MediaRepository.GetById(999).Returns((Media)null);

            // Act
            var result = _mediaService.GetMediaById(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetAllMedia_ReturnsAllMedia()
        {
            // Arrange
            var mediaList = _fixture.CreateMany<Media>(3).ToList();
            _unitOfWork.MediaRepository.GetAll().Returns(mediaList);

            // Act
            var result = _mediaService.GetAllMedia();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AddMedia_ValidMediaDto_AddsMedia()
        {
            // Arrange
            var mediaDto = _fixture.Build<MediaDTO>()
                .With(m => m.Id, 0)
                .With(m => m.FilePath, "path/to/file.jpg")
                .With(m => m.Type, "Photo")
                .With(m => m.PlaceId, 1)
                .With(m => m.UserId, 1)
                .Create();

            // Act
            _mediaService.AddMedia(mediaDto);

            // Assert
            _unitOfWork.MediaRepository.Received(1).Add(Arg.Is<Media>(m =>
                m.FilePath == "path/to/file.jpg" &&
                m.Type == "Photo" &&
                m.PlaceId == 1 &&
                m.UserId == 1
            ));
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void AddMedia_InvalidMediaDto_ThrowsArgumentException()
        {
            // Arrange
            var mediaDto = _fixture.Build<MediaDTO>()
                .With(m => m.FilePath, "")
                .With(m => m.Type, "")
                .Create();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _mediaService.AddMedia(mediaDto));
        }

        [Test]
        public void UpdateMedia_ValidMediaDto_UpdatesMedia()
        {
            // Arrange
            var existingMedia = _fixture.Build<Media>()
                .With(m => m.Id, 1)
                .Create();
            var mediaDto = _fixture.Build<MediaDTO>()
                .With(m => m.Id, 1)
                .With(m => m.FilePath, "updated/path/to/file.jpg")
                .With(m => m.Type, "Video")
                .With(m => m.PlaceId, 2)
                .With(m => m.UserId, 2)
                .Create();
            _unitOfWork.MediaRepository.GetById(1).Returns(existingMedia);

            // Act
            _mediaService.UpdateMedia(mediaDto);

            // Assert
            _unitOfWork.MediaRepository.Received(1).Update(Arg.Is<Media>(m =>
                m.Id == 1 &&
                m.FilePath == "updated/path/to/file.jpg" &&
                m.Type == "Video" &&
                m.PlaceId == 2 &&
                m.UserId == 2
            ));
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void UpdateMedia_InvalidMediaDto_ThrowsArgumentException()
        {
            // Arrange
            var mediaDto = _fixture.Build<MediaDTO>()
                .With(m => m.FilePath, "")
                .With(m => m.Type, "")
                .Create();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _mediaService.UpdateMedia(mediaDto));
        }

        [Test]
        public void DeleteMedia_ExistingId_DeletesMedia()
        {
            // Arrange
            var media = _fixture.Build<Media>().With(m => m.Id, 1).Create();
            _unitOfWork.MediaRepository.GetById(1).Returns(media);

            // Act
            _mediaService.DeleteMedia(1);

            // Assert
            _unitOfWork.MediaRepository.Received(1).Delete(media);
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void DeleteMedia_NonExistingId_DoesNotCallDelete()
        {
            // Arrange
            _unitOfWork.MediaRepository.GetById(999).Returns((Media)null);

            // Act
            _mediaService.DeleteMedia(999);

            // Assert
            _unitOfWork.MediaRepository.DidNotReceive().Delete(Arg.Any<Media>());
            _unitOfWork.DidNotReceive().SaveChanges();
        }
    }
}