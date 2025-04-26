using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Places.DAL.Repositories;
using Places.BLL.Interfaces;
using Places.BLL.Mappers;
using Places.BLL.Services;
using Places.Abstract;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Додаємо підтримку контролерів із Views
builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PlacesDbContext>(options =>
    options.UseSqlite("Data Source=places.db").UseLazyLoadingProxies());

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
            .AllowAnyMethod()
            .AllowAnyHeader();
    });
});

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPlaceService, PlaceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();

builder.Services.AddScoped<PlaceMapper>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<ReviewMapper>();
builder.Services.AddScoped<QuestionMapper>();
builder.Services.AddScoped<MediaMapper>();
builder.Services.AddScoped<AnswerMapper>();

var app = builder.Build();

app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
if (!app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseStaticFiles(); // Для статичних файлів (CSS, JS)
app.UseRouting();
app.UseAuthorization();

// Налаштування маршруту за замовчуванням для MVC
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();