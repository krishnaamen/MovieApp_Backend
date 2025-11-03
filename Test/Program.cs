using System.Net.Http;
using System.Net.Http.Json;

// Clear previous logs
Logger.Clear();

var client = new HttpClient { BaseAddress = new Uri("http://localhost:5210") };

// Run User tests
await UserTests.RunAsync(client);

// Login to get token for protected endpoints
var loginResponse = await client.PostAsJsonAsync("/api/users/login", new { Username = "testuser1", Password = "Password123!" });
var loginResult = await loginResponse.Content.ReadFromJsonAsync<UserTests.LoginResponse>();
var token = loginResult?.Token;

if (!string.IsNullOrEmpty(token))
{
    await BookmarkTests.RunAsync(client, token);
    await RatingsTests.RunAsync(client, token);
    await TitleBasicsTests.RunAsync(client, token);
}
else
{
    Logger.Log("Skipping protected tests due to missing token");
    await TitleBasicsTests.RunAsync(client);
}
