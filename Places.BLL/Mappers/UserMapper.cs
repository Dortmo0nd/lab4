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
                Full_name = user.Full_name,
                Role = user.Role,
                Password = user.Password
            };
        }

        public User ToEntity(UserDTO userDto)
        {
            if (userDto == null) return null;
            return new User
            {
                Id = userDto.Id, // Зберігаємо Id із DTO
                Full_name = userDto.Full_name,
                Role = userDto.Role,
                Password = userDto.Password
            };
        }
    }
}