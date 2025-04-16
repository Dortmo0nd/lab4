using System.Collections.Generic;
using Places.BLL.DTO;

namespace Places.BLL.Interfaces
{
    public interface IRoleService
    {
        RoleDTO GetRoleById(int id);
        IEnumerable<RoleDTO> GetAllRoles();
        void AddRole(RoleDTO role);
        void UpdateRole(RoleDTO role);
        void DeleteRole(int id);
    }
}