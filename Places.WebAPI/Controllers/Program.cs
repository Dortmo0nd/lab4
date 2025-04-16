using Microsoft.EntityFrameworkCore;
using Places.DAL.Repositories;
using Places.BLL.Interfaces;
using Places.BLL.Mappers;
using Places.BLL.Services;
using Places.Abstract;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<PlacesDbContext>(options =>
    options.UseSqlite("Data Source=places.db").UseLazyLoadingProxies());

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IPlaceService, PlaceService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IRoleService, RoleService>();
builder.Services.AddScoped<IReviewService, ReviewService>();
builder.Services.AddScoped<IQuestionService, QuestionService>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IAnswerService, AnswerService>();

builder.Services.AddScoped<PlaceMapper>();
builder.Services.AddScoped<UserMapper>();
builder.Services.AddScoped<RoleMapper>();
builder.Services.AddScoped<ReviewMapper>();
builder.Services.AddScoped<QuestionMapper>();
builder.Services.AddScoped<MediaMapper>();
builder.Services.AddScoped<AnswerMapper>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();