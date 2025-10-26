using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace MovieAppPortfolio.DataServiceLayer
{
    public interface IDataService
    {
        // TitleBasic methods
        List<TitleBasic> GetTitleBasics();
        TitleBasic? GetTitleBasicById(string tconst);
        
        // User methods
        Task<User?> RegisterUserAsync(UserRegistrationDto registrationDto);
        Task<User?> LoginUserAsync(UserLoginDto loginDto);
        Task<User?> GetUserByIdAsync(int userId);
        Task<bool> UserExistsAsync(string username, string email);
        Task<List<User>> GetAllUsersAsync();
        Task<bool> UpdateUserAsync(int userId, UserUpdateDto updateDto);
        Task<bool> DeleteUserAsync(int userId);
    }

    public class DataService : IDataService
    {
        private readonly MyDbContext _context;

        public DataService(MyDbContext context)
        {
            _context = context;
        }

        // TitleBasic methods
        public List<TitleBasic> GetTitleBasics()
        {
               return _context.Title_Basics
                .OrderBy(tb => tb.tconst)
                .Take(50)
                .ToList();
        }
        
        public TitleBasic? GetTitleBasicById(string tconst)
        {
            return _context.Title_Basics
                .FirstOrDefault(tb => tb.tconst == tconst);
        }

        // User methods
        public async Task<User?> RegisterUserAsync(UserRegistrationDto registrationDto)
        {
            if (registrationDto == null || 
                string.IsNullOrWhiteSpace(registrationDto.Username) || 
                string.IsNullOrWhiteSpace(registrationDto.Email) || 
                string.IsNullOrWhiteSpace(registrationDto.Password))
            {
                return null;
            }

            if (await UserExistsAsync(registrationDto.Username, registrationDto.Email))
                return null;

            var user = new User
            {
                Username = registrationDto.Username,
                Email = registrationDto.Email,
                Password = HashPassword(registrationDto.Password!),
                CreatedAt = DateTime.UtcNow
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User?> LoginUserAsync(UserLoginDto loginDto)
        {
            if (loginDto == null || 
                string.IsNullOrWhiteSpace(loginDto.Username) || 
                string.IsNullOrWhiteSpace(loginDto.Password))
            {
                return null;
            }

            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Username == loginDto.Username);

            if (user == null || string.IsNullOrEmpty(user.Password) || !VerifyPassword(loginDto.Password!, user.Password))
                return null;

            return user;
        }

        public async Task<User?> GetUserByIdAsync(int userId)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task<bool> UserExistsAsync(string username, string email)
        {
            if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(email))
                return false;

            return await _context.Users
                .AnyAsync(u => u.Username == username || u.Email == email);
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _context.Users
                .OrderBy(u => u.Username)
                .ToListAsync();
        }

        public async Task<bool> UpdateUserAsync(int userId, UserUpdateDto updateDto)
        {
            if (updateDto == null)
                return false;

            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            if (!string.IsNullOrWhiteSpace(updateDto.Username))
                user.Username = updateDto.Username;
            
            if (!string.IsNullOrWhiteSpace(updateDto.Email))
                user.Email = updateDto.Email;
            
            if (!string.IsNullOrWhiteSpace(updateDto.Password))
                user.Password = HashPassword(updateDto.Password);

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteUserAsync(int userId)
        {
            var user = await _context.Users.FindAsync(userId);
            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();
            return true;
        }

        private string HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
                throw new ArgumentException("Password cannot be null or empty", nameof(password));

            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                return false;

            var hash = HashPassword(password);
            return hash == storedHash;
        }
    }
}