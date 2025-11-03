using System.Net.Http.Headers;
using System.Net.Http.Json;

public static class TitleBasicsTests
{
    public static async Task RunAsync(HttpClient client, string token = null)
    {
        Logger.Log("=== Starting TitleBasics Tests ===");

        try
        {
            // Get all TitleBasics
            var allResponse = await client.GetAsync("/api/titlebasics");
            Logger.Log("GetTitleBasics response: " + await allResponse.Content.ReadAsStringAsync());

            // Get TitleBasic by Id
            var tconstExample = "tt0052520";
            var byIdResponse = await client.GetAsync($"/api/titlebasics/{tconstExample}");
            Logger.Log($"GetTitleBasicById ({tconstExample}) response: " + await byIdResponse.Content.ReadAsStringAsync());

            // Paginated
            var paginatedResponse = await client.GetAsync("/api/titlebasics/paginated?page=1&pageSize=5");
            Logger.Log("GetTitleBasicsPaginated response: " + await paginatedResponse.Content.ReadAsStringAsync());

            // Movie details
            var detailsResponse = await client.GetAsync($"/api/titlebasics/{tconstExample}/details");
            Logger.Log($"GetMovieDetails ({tconstExample}) response: " + await detailsResponse.Content.ReadAsStringAsync());

            // Database connection
            var dbTestResponse = await client.GetAsync("/api/titlebasics/testdbconnection");
            Logger.Log("TestDatabaseConnection response: " + await dbTestResponse.Content.ReadAsStringAsync());

            // JWT-protected endpoints
            if (!string.IsNullOrEmpty(token))
            {
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

                var keyword = "matrix";

                // Search
                var searchResponse = await client.GetAsync($"/api/titlebasics/search/{keyword}");
                Logger.Log($"BestMatchSearchSingle ('{keyword}') response: " + await searchResponse.Content.ReadAsStringAsync());

                // Search history
                var historyResponse = await client.GetAsync("/api/titlebasics/search/history");
                Logger.Log("GetSearchHistory response: " + await historyResponse.Content.ReadAsStringAsync());
            }
            else
            {
                Logger.Log("Skipping protected endpoints due to missing token.");
            }
        }
        catch (Exception ex)
        {
            Logger.Log("TitleBasics tests exception: " + ex.Message);
        }
    }
}
