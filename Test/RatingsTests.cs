using System.Net.Http.Headers;
using System.Net.Http.Json;

public static class RatingsTests
{
    public static async Task RunAsync(HttpClient client, string token)
    {
        Logger.Log("=== Starting Rating Tests ===");

        try
        {
            // Set token for Authorization
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            // Rate a movie
            var rateDto = new { TConst = "tt0052520", Rating = 8.5 };
            var rateResponse = await client.PostAsJsonAsync("/api/Ratings", rateDto);
            Logger.Log("Rate movie response: " + await rateResponse.Content.ReadAsStringAsync());

            //Get ratings
            var myRatingsResponse = await client.GetAsync("/api/Ratings/my-ratings");
            Logger.Log("My ratings response: " + await myRatingsResponse.Content.ReadAsStringAsync());

            // Update rating
            var updateDto = new { Rating = 9.0 };
            var updateResponse = await client.PutAsJsonAsync("/api/Ratings/tt0052520", updateDto);
            Logger.Log("Update rating response: " + await updateResponse.Content.ReadAsStringAsync());

            //  Get rating for a movie
            var movieRatingResponse = await client.GetAsync("/api/Ratings/movie/tt0052520");
            Logger.Log("Get rating for movie response: " + await movieRatingResponse.Content.ReadAsStringAsync());

            // Remove rating
            var removeResponse = await client.DeleteAsync("/api/Ratings/tt0052520");
            Logger.Log("Remove rating response: " + await removeResponse.Content.ReadAsStringAsync());
        }
        catch (Exception ex)
        {
            Logger.Log("Rating tests exception: " + ex.Message);
        }
    }
}
