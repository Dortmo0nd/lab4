using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Places.Abstract;
using Places.BLL.Interfaces;
using Places.BLL.Mappers;
using Places.BLL.Services;
using Places.DAL.Repositories;
using Places.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Настройка контроллеров и сериализации JSON
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

// Подключение Swagger для API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Настройка контекста базы данных с использованием SQLite
builder.Services.AddDbContext<PlacesDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));

// Настройка аутентификации через cookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Index";
        //options.AccessDeniedPath = "/Users/AccessDenied";
    });

// Регистрация сервисов и репозиториев
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPlaceService, PlaceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();

// Регистрация мапперов
builder.Services.AddScoped<PlaceMapper>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<ReviewMapper>();
builder.Services.AddScoped<QuestionMapper>();
builder.Services.AddScoped<MediaMapper>();
builder.Services.AddScoped<AnswerMapper>();

// Подключение Razor Pages
builder.Services.AddRazorPages();

// Создание приложения
var app = builder.Build();

// Настройка middleware для разработки
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Настройка обработки статических файлов, маршрутизации и авторизации
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Настройка маршрута по умолчанию
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

// Подключение Razor Pages
app.MapRazorPages();

// Запуск приложения
app.Run();