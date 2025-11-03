using System.Net.Http.Json;

public static class UserTests
{
    public static async Task RunAsync(HttpClient client)
    {
        Logger.Log("=== Starting User Tests ===");

        try
        {
            // Register
            var registerData = new
            {
                Username = "testuser1",
                Email = "testuser1@example.com",
                Password = "Password123!"
            };

            var registerResponse = await client.PostAsJsonAsync("/api/users/register", registerData);
            if (registerResponse.IsSuccessStatusCode)
            {
                Logger.Log("Register success");
            }
            else
            {
                Logger.Log($"Register failed: {registerResponse.StatusCode}");
                Logger.Log(await registerResponse.Content.ReadAsStringAsync());
            }

            // Login
            var loginData = new
            {
                Username = "testuser1",
                Password = "Password123!"
            };

            var loginResponse = await client.PostAsJsonAsync("/api/users/login", loginData);
            if (loginResponse.IsSuccessStatusCode)
            {
                var loginResult = await loginResponse.Content.ReadFromJsonAsync<LoginResponse>();
                Logger.Log("Login success, token: " + loginResult?.Token);
            }
            else
            {
                Logger.Log($"Login failed: {loginResponse.StatusCode}");
                Logger.Log(await loginResponse.Content.ReadAsStringAsync());
            }
        }
        catch (Exception ex)
        {
            Logger.Log("User tests exception: " + ex.Message);
        }
    }

    public record LoginResponse(string Token);
}
