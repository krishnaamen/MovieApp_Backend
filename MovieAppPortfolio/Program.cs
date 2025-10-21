//using DotNetEnv;

//Env.Load();
//var connectionString = Environment.GetEnvironmentVariable("CONNECTION_STRING");
//Console.WriteLine($"Connection String: {connectionString}");

using MovieAppPortfolio.DataServiceLayer;

var builder = WebApplication.CreateBuilder(args);



builder.Services.AddControllers();

builder.Services.AddDbContext<MyDbContext>();


builder.Services.AddScoped<DataService>();





builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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
