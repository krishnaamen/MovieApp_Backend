using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using MovieAppPortfolio.DataServiceLayer;
using DataServiceLayer.Services.UserRatingServices;

var builder = WebApplication.CreateBuilder(args);

// Load environment variables
var envPath = Path.Combine(Directory.GetCurrentDirectory(), ".env");
if (File.Exists(envPath))
{
    Env.Load(envPath);
    Console.WriteLine(" .env file loaded");
}
else
{
    Console.WriteLine($" .env file not found at {envPath}");
}

//  Get connection string
var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
if (string.IsNullOrEmpty(connectionString))
{
    throw new InvalidOperationException("DB_CONNECTION not found in .env file!");
}

// Add database and services
builder.Services.AddDbContext<MyDbContext>(options =>
    options.UseNpgsql(connectionString));

builder.Services.AddScoped<IDataService, DataService>();

// Register  rating repository service
builder.Services.AddScoped<IRatingRepository, UserRatingServices>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Run app
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
