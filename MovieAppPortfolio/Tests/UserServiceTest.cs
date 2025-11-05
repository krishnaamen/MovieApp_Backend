using Microsoft.EntityFrameworkCore;
using MovieAppPortfolio.DataServiceLayer;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Xunit;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Tests
{
    public class AuthApiTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;
        private readonly MyDbContext _context;

        public AuthApiTests()
        {
            // Create WebApplicationFactory with in-memory database configuration
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(builder =>
                {
                    builder.ConfigureServices(services =>
                    {
                        // Remove the existing DbContext registration
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<MyDbContext>));
                        if (descriptor != null)
                        {
                            services.Remove(descriptor);
                        }

                        // Add in-memory database
                        services.AddDbContext<MyDbContext>(options =>
                        {
                            options.UseInMemoryDatabase($"AuthTestDb_{Guid.NewGuid()}");
                        });
                    });
                });

            _client = factory.CreateClient();

            // Get the DbContext from the service provider
            var scope = factory.Services.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<MyDbContext>();
        }

        [Fact]
        public async Task Register_ValidUser_ReturnsSuccess()
        {
            // Arrange
            var registerData = new
            {
                username = $"testuser_{Guid.NewGuid()}",
                email = $"test{Guid.NewGuid()}@gmail.com",
                password = "TestPassword123!"
            };

            var json = JsonSerializer.Serialize(registerData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Users/register", content);

            // Assert
            response.EnsureSuccessStatusCode();
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            
        }

        [Fact]
        
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange - Register a user first
            var username = "testuser13";
            var password = "TestPassword123!";
            var email = $"{username}@example.com";

            var registerData = new
            {
                username = username,
                email = email,
                password = password
            };

            var registerJson = JsonSerializer.Serialize(registerData);
            var registerContent = new StringContent(registerJson, Encoding.UTF8, "application/json");

            // Register and verify success with detailed debugging
            var registerResponse = await _client.PostAsync("/api/Users/register", registerContent);

            if (!registerResponse.IsSuccessStatusCode)
            {
                var registerError = await registerResponse.Content.ReadAsStringAsync();
                throw new Exception($"REGISTRATION FAILED ({registerResponse.StatusCode}): {registerError}");
            }

            registerResponse.EnsureSuccessStatusCode();

            // Debug: Check what registration actually returned
            var registerResponseContent = await registerResponse.Content.ReadAsStringAsync();
            Console.WriteLine($"Registration Response: {registerResponseContent}");

            // Increased delay to ensure user is persisted and available
            await Task.Delay(500);

            // Act - Login with valid credentials
            var loginData = new
            {
                username = "testuser13",
                password = "TestPassword123"
            };

            var loginJson = JsonSerializer.Serialize(loginData);
            var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Users/login", loginContent);

            // Debug: Check what the login response actually contains if it fails
            if (!response.IsSuccessStatusCode)
            {
                var loginError = await response.Content.ReadAsStringAsync();
                throw new Exception($"LOGIN FAILED ({response.StatusCode}): {loginError}");
            }

            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseContent);

            // Check if token exists in the response
            Assert.True(doc.RootElement.TryGetProperty("token", out JsonElement tokenElement));
            Assert.False(string.IsNullOrEmpty(tokenElement.GetString()));
        }

        [Fact]
        public async Task Login_InvalidCredentials_ReturnsUnauthorized()
        {
            // Arrange
            var loginData = new
            {
                username = "nonexistentuser",
                password = "WrongPassword123!"
            };

            var json = JsonSerializer.Serialize(loginData);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            // Act
            var response = await _client.PostAsync("/api/Users/login", content);

            // Assert
            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        public void Dispose()
        {
            _context?.Dispose();
            _client?.Dispose();
        }
    }
}