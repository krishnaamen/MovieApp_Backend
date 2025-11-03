using System.Net.Http.Headers;
using System.Net.Http.Json;

public static class BookmarkTests
{
    public static async Task RunAsync(HttpClient client, string token)
    {
        Logger.Log("=== Starting Bookmark Tests ===");

        try
        {
            // Set token for Authorization
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Debug token
            var debugResponse = await client.GetAsync("/api/Bookmarks/debug/token");
            Logger.Log("Debug token response: " + await debugResponse.Content.ReadAsStringAsync());

            // Add bookmark
            var bookmark = new { TConst = "tt0052520", NConst = "nm7823096" };
            var addResponse = await client.PostAsJsonAsync("/api/Bookmarks", bookmark);
            Logger.Log("Add bookmark response: " + await addResponse.Content.ReadAsStringAsync());

            // Check bookmark
            var checkResponse = await client.GetAsync("/api/Bookmarks/check/tt1234567");
            Logger.Log("Check bookmark response: " + await checkResponse.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            Logger.Log("Bookmark tests exception: " + ex.Message);
        }
    }
}
