<<<<<<< HEAD
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
=======
using Mapster;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
>>>>>>> bhisma/auth-test
using MovieAppPortfolio.DataServiceLayer;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using MovieAppPortfolio.DataServiceLayer.user;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddControllers();
<<<<<<< HEAD
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Simple DbContext registration without options (if you don't have the constructor)
builder.Services.AddDbContext<MyDbContext>();

// Or if you need to configure the connection string, use this approach:
// builder.Services.AddDbContext<MyDbContext>(options => 
//     options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Register DataService
builder.Services.AddScoped<DataService>();
builder.Services.AddScoped(typeof(IDataService), typeof(DataService));

// Configure JWT Authentication
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "your-super-secret-key-at-least-32-characters-long-here";
=======

builder.Services.AddDbContext<MyDbContext>();


builder.Services.AddScoped<IDataService, DataService>();
builder.Services.AddScoped<IUsersDataservice, UsersDataService>();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Configure JWT Authentication
var jwtSecret = builder.Configuration["Jwt:Secret"] ?? "This is a secret key where it should have to be more than 32 character";
>>>>>>> bhisma/auth-test
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtSecret)),
            ValidateIssuer = false,
            ValidateAudience = false,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
    });

builder.Services.AddAuthorization();

<<<<<<< HEAD
=======



>>>>>>> bhisma/auth-test
var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseAuthentication();
<<<<<<< HEAD
=======

>>>>>>> bhisma/auth-test
app.UseAuthorization();
app.MapControllers();

<<<<<<< HEAD
app.Run();
=======
app.Run();

public partial class Program { }
>>>>>>> bhisma/auth-test
