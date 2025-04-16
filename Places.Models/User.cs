using System.Collections.Generic;

namespace Places.Models
{
    public class User
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public ICollection<Review> Reviews { get; set; }
        public ICollection<Question> Questions { get; set; }
        public ICollection<Media> MediaFiles { get; set; }
    }
}