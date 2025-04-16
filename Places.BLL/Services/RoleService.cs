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
    public class RoleService : IRoleService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly RoleMapper _mapper;

        public RoleService(IUnitOfWork unitOfWork, RoleMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public RoleDTO GetRoleById(int id)
        {
            var role = _unitOfWork.RoleRepository.GetById(id);
            return _mapper.ToDto(role);
        }

        public IEnumerable<RoleDTO> GetAllRoles()
        {
            var roles = _unitOfWork.RoleRepository.GetAll();
            return roles.Select(r => _mapper.ToDto(r));
        }

        public void AddRole(RoleDTO roleDto)
        {
            if (roleDto == null || string.IsNullOrEmpty(roleDto.Name))
                throw new ArgumentException("Invalid role data");

            var role = _mapper.ToEntity(roleDto);
            _unitOfWork.RoleRepository.Add(role);
            _unitOfWork.SaveChanges();
        }

        public void UpdateRole(RoleDTO roleDto)
        {
            if (roleDto == null || string.IsNullOrEmpty(roleDto.Name))
                throw new ArgumentException("Invalid role data");

            var role = _mapper.ToEntity(roleDto);
            _unitOfWork.RoleRepository.Update(role);
            _unitOfWork.SaveChanges();
        }

        public void DeleteRole(int id)
        {
            var role = _unitOfWork.RoleRepository.GetById(id);
            if (role != null)
            {
                _unitOfWork.RoleRepository.Delete(role);
                _unitOfWork.SaveChanges();
            }
        }
    }
}