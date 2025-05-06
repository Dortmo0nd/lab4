using Places.Models;
using System.ComponentModel.DataAnnotations;

namespace Places.BLL.DTO
{
    public class UserDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Full name is required")]
        public string Full_name { get; set; }

        [Required(ErrorMessage = "Role is required")]
        public User.UserRole Role { get; set; }

        [Required(ErrorMessage = "Password is required")]
        public string Password { get; set; }
    }
}