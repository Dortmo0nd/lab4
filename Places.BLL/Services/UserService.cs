using System;
using System.Collections.Generic;
using System.Linq;
using Places.BLL.DTO;
using Places.BLL.Interfaces;
using Places.BLL.Mappers;
using Places.Abstract;
using Places.Models;
using BCrypt.Net;

namespace Places.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserMapper _mapper;

        public UserService(IUnitOfWork unitOfWork, UserMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public UserDTO GetUserById(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            return _mapper.ToDto(user);
        }

        public IEnumerable<UserDTO> GetAllUsers()
        {
            var users = _unitOfWork.UserRepository.GetAll();
            return users.Select(u => _mapper.ToDto(u));
        }

        public void AddUser(UserDTO userDto)
        {
            if (userDto == null || string.IsNullOrEmpty(userDto.Password))
                throw new ArgumentException("Invalid user data");

            Console.WriteLine($"Adding user: {userDto.Full_name}, Role: {userDto.Role}");
            Console.WriteLine($"Received UserDTO: Id={userDto.Id}, Name={userDto.Full_name}, Role={userDto.Role}, Password={userDto.Password}");
            var user = _mapper.ToEntity(userDto);
            Console.WriteLine($"Mapped User: Id={user.Id}, Name={user.Full_name}, Role={user.Role}");
            user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.SaveChanges();
            Console.WriteLine("User saved with Id: " + user.Id);
        }

        public void UpdateUser(UserDTO userDto)
        {
            if (userDto == null || string.IsNullOrEmpty(userDto.Password))
                throw new ArgumentException("Invalid user data");

            var user = _mapper.ToEntity(userDto);
            user.Password = BCrypt.Net.BCrypt.HashPassword(userDto.Password);
            _unitOfWork.UserRepository.Update(user);
            _unitOfWork.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = _unitOfWork.UserRepository.GetById(id);
            if (user != null)
            {
                _unitOfWork.UserRepository.Delete(user);
                _unitOfWork.SaveChanges();
            }
        }
        public bool VerifyPassword(int userId, string password)
        {
            var user = _unitOfWork.UserRepository.GetById(userId);
            if (user == null)
                return false;

            return BCrypt.Net.BCrypt.Verify(password, user.Password);
        }
        
        public UserDTO GetUserByUsername(string username)
        {
            var user = _unitOfWork.UserRepository.Find(u => u.Full_name == username).FirstOrDefault();
            return _mapper.ToDto(user);
        }
    }
}