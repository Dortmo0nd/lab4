using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoFixture;
using NSubstitute;
using NUnit.Framework;
using Places.Abstract;
using Places.BLL.DTO;
using Places.BLL.Mappers;
using Places.BLL.Services;
using Places.Models;
using Assert = NUnit.Framework.Assert;

namespace Places.test_services
{
    [TestFixture]
    public class UserServiceTests
    {
        private UserService _userService;
        private IUnitOfWork _unitOfWork;
        private UserMapper _userMapper;
        private Fixture _fixture;

        [SetUp]
        public void SetUp()
        {
            _unitOfWork = Substitute.For<IUnitOfWork>();
            _userMapper = new UserMapper();
            _fixture = new Fixture();
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList()
                .ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            _unitOfWork.UserRepository.Returns(Substitute.For<IRepository<User>>());
            _userService = new UserService(_unitOfWork, _userMapper);
        }

        [Test]
        public void GetUserById_ExistingId_ReturnsUserDto()
        {
            // Arrange
            var user = _fixture.Build<User>()
                .With(u => u.Id, 1)
                .With(u => u.Reviews, new List<Review>())
                .With(u => u.Answers, new List<Answer>())
                .With(u => u.MediaFiles, new List<Media>())
                .Create();
            _unitOfWork.UserRepository.GetById(1).Returns(user);

            // Act
            var result = _userService.GetUserById(1);

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Id, Is.EqualTo(1));
            Assert.That(result.Full_name, Is.EqualTo(user.Full_name));
        }

        [Test]
        public void GetUserById_NonExistingId_ReturnsNull()
        {
            // Arrange
            _unitOfWork.UserRepository.GetById(999).Returns((User)null);

            // Act
            var result = _userService.GetUserById(999);

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void GetAllUsers_ReturnsAllUsers()
        {
            // Arrange
            var users = _fixture.CreateMany<User>(3).ToList();
            _unitOfWork.UserRepository.GetAll().Returns(users);

            // Act
            var result = _userService.GetAllUsers();

            // Assert
            Assert.That(result.Count(), Is.EqualTo(3));
        }

        [Test]
        public void AddUser_ValidUserDto_AddsUser()
        {
            // Arrange
            var userDto = _fixture.Build<UserDTO>()
                .With(u => u.Id, 0)
                .With(u => u.Full_name, "Test User")
                .With(u => u.Password, "password123")
                .Create();

            // Act
            _userService.AddUser(userDto);

            // Assert
            _unitOfWork.UserRepository.Received(1).Add(Arg.Any<User>());
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void AddUser_InvalidUserDto_ThrowsArgumentException()
        {
            // Arrange
            var userDto = _fixture.Build<UserDTO>()
                .With(u => u.Password, "")
                .Create();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => _userService.AddUser(userDto));
        }

        [Test]
        public void UpdateUser_ValidUserDto_UpdatesUser()
        {
            // Arrange
            var existingUser = _fixture.Build<User>()
                .With(u => u.Id, 1)
                .Create();
            var userDto = _fixture.Build<UserDTO>()
                .With(u => u.Id, 1)
                .With(u => u.Full_name, "Updated User")
                .With(u => u.Password, "newpassword")
                .Create();
            _unitOfWork.UserRepository.GetById(1).Returns(existingUser);

            // Act
            _userService.UpdateUser(userDto);

            // Assert
            _unitOfWork.UserRepository.Received(1).Update(Arg.Any<User>());
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void UpdateUser_NonExistingUser_ThrowsException()
        {
            // Arrange
            var userDto = _fixture.Build<UserDTO>()
                .With(u => u.Id, 999)
                .With(u => u.Password, "password123")
                .Create();
            _unitOfWork.UserRepository.GetById(999).Returns((User)null);

            // Act & Assert
            Assert.Throws<Exception>(() => _userService.UpdateUser(userDto));
        }

        [Test]
        public void DeleteUser_ExistingId_DeletesUser()
        {
            // Arrange
            var user = _fixture.Build<User>().With(u => u.Id, 1).Create();
            _unitOfWork.UserRepository.GetById(1).Returns(user);

            // Act
            _userService.DeleteUser(1);

            // Assert
            _unitOfWork.UserRepository.Received(1).Delete(user);
            _unitOfWork.Received(1).SaveChanges();
        }

        [Test]
        public void GetUserByUsername_ExistingUsername_ReturnsUserDto()
        {
            // Arrange
            var user = _fixture.Build<User>()
                .With(u => u.Full_name, "TestUser")
                .Create();
            _unitOfWork.UserRepository.Find(Arg.Any<Expression<Func<User, bool>>>())
                .Returns(new List<User> { user });

            // Act
            var result = _userService.GetUserByUsername("TestUser");

            // Assert
            Assert.That(result, Is.Not.Null);
            Assert.That(result.Full_name, Is.EqualTo("TestUser"));
        }

        [Test]
        public void GetUserByUsername_NonExistingUsername_ReturnsNull()
        {
            // Arrange
            _unitOfWork.UserRepository.Find(Arg.Any<Expression<Func<User, bool>>>())
                .Returns(new List<User>());

            // Act
            var result = _userService.GetUserByUsername("NonExistingUser");

            // Assert
            Assert.That(result, Is.Null);
        }

        [Test]
        public void VerifyPassword_CorrectPassword_ReturnsTrue()
        {
            // Arrange
            var user = _fixture.Build<User>()
                .With(u => u.Id, 1)
                .With(u => u.Password, BCrypt.Net.BCrypt.HashPassword("password123"))
                .Create();
            _unitOfWork.UserRepository.GetById(1).Returns(user);

            // Act
            var result = _userService.VerifyPassword(1, "password123");

            // Assert
            Assert.That(result, Is.True);
        }

        [Test]
        public void VerifyPassword_IncorrectPassword_ReturnsFalse()
        {
            // Arrange
            var user = _fixture.Build<User>()
                .With(u => u.Id, 1)
                .With(u => u.Password, BCrypt.Net.BCrypt.HashPassword("password123"))
                .Create();
            _unitOfWork.UserRepository.GetById(1).Returns(user);

            // Act
            var result = _userService.VerifyPassword(1, "wrongpassword");

            // Assert
            Assert.That(result, Is.False);
        }

        [Test]
        public void VerifyPassword_NonExistingUser_ReturnsFalse()
        {
            // Arrange
            _unitOfWork.UserRepository.GetById(999).Returns((User)null);

            // Act
            var result = _userService.VerifyPassword(999, "password123");

            // Assert
            Assert.That(result, Is.False);
        }
    }
}