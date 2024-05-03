using Common;
using Common.Interfaces;
using Common.Profiles;
using Data.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var config = builder.Configuration;

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//db
builder.Services.AddDbContext<DigitalKhataDbContext>(options =>
{
    options.UseSqlServer(config.GetConnectionString("DefaultConnection"));
});

//automapper
builder.Services.AddAutoMapper(typeof(AutomapperProfiles).Assembly);

//services
builder.Services.AddScoped<IUserRepository, UserRepository>();

//mediatr
builder.Services.AddMediatR(c =>
    c.RegisterServicesFromAssemblyContaining<Program>()
);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
