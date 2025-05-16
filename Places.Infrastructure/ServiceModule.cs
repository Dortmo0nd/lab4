using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Places.Abstract;
using Places.BLL.Interfaces;
using Places.BLL.Mappers;
using Places.BLL.Services;
using Places.DAL.Repositories;


namespace Places.Infrastructure
{
    public static class ServiceModule
    {
        /// <summary>
        /// Реєструє сервіси бізнес-логіки (BLL) у контейнері залежностей.
        /// </summary>
        public static IServiceCollection AddPlacesBLL(this IServiceCollection services)
        {
            services.AddScoped<IAnswerService, AnswerService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IUserService, UserService>();

            // Реєстрація маперів як залежностей
            services.AddScoped<AnswerMapper>();
            services.AddScoped<MediaMapper>();
            services.AddScoped<PlaceMapper>();
            services.AddScoped<QuestionMapper>();
            services.AddScoped<ReviewMapper>();
            services.AddScoped<UserMapper>();

            return services;
        }

        /// <summary>
        /// Реєструє компоненти доступу до даних (DAL) у контейнері залежностей.
        /// </summary>
        public static IServiceCollection AddPlacesDal(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<PlacesDbContext>(options =>
                options.UseSqlite(connectionString)
                       .UseLazyLoadingProxies());

            services.AddScoped<PlacesDbContext>(provider => provider.GetRequiredService<PlacesDbContext>());
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}