using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Places.DAL.Repositories; // Adjust namespaces as needed
using Places.BLL.Interfaces;
using Places.BLL.Mappers;
using Places.BLL.Services;
using Places.Abstract;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// 1. Register Services (BEFORE builder.Build())
builder.Services.AddControllersWithViews();

// Add Authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Users/Login";
        options.LogoutPath = "/Users/Logout";
    });

// Add DbContext
builder.Services.AddDbContext<PlacesDbContext>(options => 
    options.UseSqlite("Data Source=places.db"));

// Add UnitOfWork and Services
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPlaceService, PlaceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();


// Add Mappers
builder.Services.AddScoped<PlaceMapper>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<ReviewMapper>();
builder.Services.AddScoped<QuestionMapper>();
builder.Services.AddScoped<MediaMapper>();
builder.Services.AddScoped<AnswerMapper>();

// Add other services
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. Build the Application
var app = builder.Build();

// 3. Configure Middleware (AFTER builder.Build())
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
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();