using System.Collections.Generic;

namespace Places.Models
{
    public class User
    {
        public int Id { get; set; }
        public int RoleId { get; set; } // Зовнішній ключ для Role
        public Role Role { get; set; }  // Навігаційна властивість
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Answer> Answers { get; set; } // Відповідає 'u => u.Answers'
        public ICollection<Media> MediaFiles { get; set; }
    }
}