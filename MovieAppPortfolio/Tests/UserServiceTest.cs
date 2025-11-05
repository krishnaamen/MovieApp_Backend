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

namespace Tests
{
    public class AuthApiTests : IClassFixture<WebApplicationFactory<Program>>, IDisposable
    {
        private readonly HttpClient _client;
        private readonly MyDbContext _context;

        public AuthApiTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
            
            // Set up in-memory database
            var options = new DbContextOptionsBuilder<MyDbContext>()
                .UseInMemoryDatabase(databaseName: $"AuthTestDb_{Guid.NewGuid()}")
                .Options;
            
            _context = new MyDbContext(options);
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
            
            var responseContent = await response.Content.ReadAsStringAsync();
            Assert.Contains("success", responseContent.ToLower());
        }

        [Fact]
        public async Task Login_ValidCredentials_ReturnsToken()
        {
            // Arrange - Register a user first
            var username = $"testuser_{Guid.NewGuid()}";
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
            await _client.PostAsync("/api/Users/register", registerContent);

            // Act - Login with valid credentials
            var loginData = new
            {
                username = username,
                password = password
            };

            var loginJson = JsonSerializer.Serialize(loginData);
            var loginContent = new StringContent(loginJson, Encoding.UTF8, "application/json");
            var response = await _client.PostAsync("/api/Users/login", loginContent);

            // Assert
            response.EnsureSuccessStatusCode();
            
            var responseContent = await response.Content.ReadAsStringAsync();
            using JsonDocument doc = JsonDocument.Parse(responseContent);
            
            Assert.True(doc.RootElement.TryGetProperty("token", out _));
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
        }
    }
}