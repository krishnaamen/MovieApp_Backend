using System.Net.Http.Json;

var baseUrl = "http://localhost:5210/"; 
var client = new HttpClient { BaseAddress = new Uri(baseUrl) };

// REGISTER
Console.WriteLine("=== Registering ===");

var registerData = new
{
    Username = "testuser1",
    Email = "testuser1@example.com",
    Password = "Password123!"
};

var registerResponse = await client.PostAsJsonAsync("/api/Users/registers", registerData);
Console.WriteLine(await registerResponse.Content.ReadAsStringAsync());

// LOGIN
Console.WriteLine("\n=== Logging in ===");

var loginData = new
{
    Username = "testuser1",
    Password = "Password123!"
};

var loginResponse = await client.PostAsJsonAsync("/api/users/login", loginData);
Console.WriteLine(await loginResponse.Content.ReadAsStringAsync());
