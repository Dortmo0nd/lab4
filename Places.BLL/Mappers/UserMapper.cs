using Places.Models;
using Places.BLL.DTO;

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
                Role = user.Role,
                Password = user.Password
            };
        }

        public User ToEntity(UserDTO userDto)
        {
            if (userDto == null) return null;
            return new User
            {
                Id = userDto.Id,
                Role = userDto.Role,
                Password = userDto.Password
            };
        }
    }
}