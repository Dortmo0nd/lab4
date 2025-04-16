using Places.BLL.DTO;
using Places.Models;

namespace Places.BLL.Mappers
{
    public class RoleMapper
    {
        public RoleDTO ToDto(Role role)
        {
            if (role == null) return null;
            return new RoleDTO
            {
                Id = role.Id,
                Name = role.Name
            };
        }

        public Role ToEntity(RoleDTO roleDto)
        {
            if (roleDto == null) return null;
            return new Role
            {
                Id = roleDto.Id,
                Name = roleDto.Name
            };
        }
    }
}