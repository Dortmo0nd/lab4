using Places.Models;

namespace Places.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }
        public string Full_name { get; set; }
        public User.UserRole Role { get; set; }
        public string Password { get; set; }
    }
}