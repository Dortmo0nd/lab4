using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Places.Abstract;
using Places.DAL.Repositories;

namespace Places.Infrastructure
{
    public static class PlacesDalDependencyRegistration
    {
        public static IServiceCollection AddPlacesDal(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PlacesDbContext>(options =>
                options.UseSqlite(connectionString).UseLazyLoadingProxies());

            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}