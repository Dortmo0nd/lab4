using System.Collections.Generic;

namespace Places.Models
{
    public class User
    {
        public int Id { get; set; }
        public UserRole Role { get; set; }
        public string Password { get; set; }
        public virtual ICollection<Review> Reviews { get; set; }
        public virtual ICollection<Answer> Answers { get; set; }
        public virtual ICollection<Media> MediaFiles { get; set; }
        
        public enum UserRole
        {
            Visitor,
            Admin
        }
    }
}