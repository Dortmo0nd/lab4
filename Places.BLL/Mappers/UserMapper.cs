using Places.BLL.DTO;
using Places.Models;

namespace Places.BLL.Mappers
{
    public class UserMapper
    {
        public UserDTO ToDto(User user)
        {
            if (user == null) return null;
            return new UserDTO
            {
                Id = user.Id,
                RoleId = user.RoleId
            };
        }

        public User ToEntity(UserDTO userDto)
        {
            if (userDto == null) return null;
            return new User
            {
                Id = userDto.Id,
                RoleId = userDto.RoleId
            };
        }
    }
}