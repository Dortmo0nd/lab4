using Microsoft.Extensions.DependencyInjection;
using Places.BLL.Interfaces;
using Places.BLL.Mappers;
using Places.BLL.Services;

namespace Places.Infrastructure
{
    public static class PlacesBllDependencyRegistration
    {
        public static IServiceCollection AddPlacesBll(this IServiceCollection services)
        {
            services.AddScoped<IPlaceService, PlaceService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IReviewService, ReviewService>();
            services.AddScoped<IQuestionService, QuestionService>();
            services.AddScoped<IMediaService, MediaService>();
            services.AddScoped<IAnswerService, AnswerService>();

            services.AddScoped<PlaceMapper>();
            services.AddScoped<UserMapper>();
            services.AddScoped<ReviewMapper>();
            services.AddScoped<QuestionMapper>();
            services.AddScoped<MediaMapper>();
            services.AddScoped<AnswerMapper>();

            return services;
        }
    }
}