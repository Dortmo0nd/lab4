using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Places.DAL.Repositories;

namespace Places.DAL.Repositories
{
    public class PlacesDbContextFactory : IDesignTimeDbContextFactory<PlacesDbContext>
    {
        public PlacesDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<PlacesDbContext>();
            optionsBuilder.UseSqlite("Data Source=places.db");

            return new PlacesDbContext(optionsBuilder.Options);
        }
    }
}