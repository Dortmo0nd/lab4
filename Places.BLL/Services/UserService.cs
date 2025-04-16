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
            if (userDto == null || userDto.RoleId <= 0)
                throw new ArgumentException("Invalid user data");

            var user = _mapper.ToEntity(userDto);
            _unitOfWork.UserRepository.Add(user);
            _unitOfWork.SaveChanges();
        }

        public void UpdateUser(UserDTO userDto)
        {
            if (userDto == null || userDto.RoleId <= 0)
                throw new ArgumentException("Invalid user data");

            var user = _mapper.ToEntity(userDto);
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
    }
}