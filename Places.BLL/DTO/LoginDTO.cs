using System.ComponentModel.DataAnnotations;

namespace Places.BLL.DTO
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Введіть ім’я користувача")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Введіть пароль")]
        public string Password { get; set; }
    }
}