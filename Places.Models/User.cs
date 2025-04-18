using System.Collections.Generic;

namespace Places.Models
{
    public class User
    {
        public int Id { get; set; }
        public UserRole Role { get; set; } // Нова властивість role типу UserRole
        public string Password { get; set; } // Нова властивість password
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public ICollection<Media> MediaFiles { get; set; }
        
        public enum UserRole
        {
            Visitor,
            Admin
        }
    }
}