namespace Places.Models;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } // User, Manager
    public ICollection<User> Users { get; set; }
}