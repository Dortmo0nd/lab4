using System.Collections.Generic;
using Places.BLL.DTO;

namespace Places.BLL.Interfaces
{
    public interface IUserService
    {
        UserDTO GetUserById(int id);
        IEnumerable<UserDTO> GetAllUsers();
        void AddUser(UserDTO user);
        void UpdateUser(UserDTO user);
        void DeleteUser(int id);
    }
}