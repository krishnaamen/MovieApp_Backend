using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace MovieAppPortfolio
{
    public class UserTest
    {
        private readonly HttpClient _client;

        public UserTest(HttpClient client)
        {
            _client = client;

            //  BaseAddress set
            if (_client.BaseAddress == null)
            {
                _client.BaseAddress = new Uri("http://localhost:5210/");
            }
        }

        public async Task<bool> RegisterAndLoginAsync()
        {
            var registerData = new
            {
                Username = "testuser1",
                Email = "testuser1@example.com",
                Password = "Password123!"
            };

            var registerResponse = await _client.PostAsJsonAsync("/api/Users/registers", registerData);
            if (!registerResponse.IsSuccessStatusCode)
                return false;

            var loginData = new
            {
                Username = "testuser1",
                Password = "Password123!"
            };

            var loginResponse = await _client.PostAsJsonAsync("/api/users/login", loginData);
            return loginResponse.IsSuccessStatusCode;
        }
    }
}
