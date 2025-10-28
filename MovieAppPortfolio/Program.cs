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
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo
    {
        Title = "Movie App API",
        Version = "v1",
        Description = "API for managing movies, bookmarks, and related operations",
        Contact = new Microsoft.OpenApi.Models.OpenApiContact
        {
            Name = "API Support",
            Email = "support@movieapp.com"
        }
    });

    // Include XML comments
    var xmlFile = $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";
    var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
    c.IncludeXmlComments(xmlPath);

    // Add response types for standardization
    c.UseAllOfToExtendReferenceSchemas();
});

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
