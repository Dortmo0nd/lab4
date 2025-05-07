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

builder.Services.AddControllersWithViews().AddJsonOptions(options =>
{
    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PlacesDbContext>(options =>
    options.UseSqlite("Data Source=places.db").UseLazyLoadingProxies());

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Index";
        //options.AccessDeniedPath = "/Users/AccessDenied";
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

builder.Services.AddRazorPages();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Index}/{id?}");

app.MapRazorPages();

app.Run();