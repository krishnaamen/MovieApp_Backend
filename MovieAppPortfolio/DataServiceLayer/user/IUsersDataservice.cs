namespace MovieAppPortfolio.DataServiceLayer.user
{
    public interface IUsersDataservice
    {

        Task<User?> RegisterUserAsync(UserRegistrationDto registrationDto);
        Task<User?> LoginUserAsync(UserLoginDto loginDto);
        Task<User?> GetUserByIdAsync(int userId);
        Task<bool> UserExistsAsync(string username, string email);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(int userId, UserUpdateDto updateDto);
        Task<bool> DeleteUserAsync(int userId);
    }
}
