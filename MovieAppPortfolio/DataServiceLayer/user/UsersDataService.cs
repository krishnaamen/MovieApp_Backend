using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace MovieAppPortfolio.DataServiceLayer.user
{
    public class UsersDataService: IUsersDataservice
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public UsersDataService(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }



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

            

            return new User
            {
                UserId = user.UserId,
                Username = user.Username,
                Email = user.Email,
                CreatedAt = user.CreatedAt
            }; 
        }

        private string GenerateJwtToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration.GetSection("AppSettings:Token").Value!);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), // THIS IS CRITICAL
                    new Claim(ClaimTypes.Name, user.Username ?? "unknown"),
                    new Claim(ClaimTypes.Email, user.Email ?? "")
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
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

            try
            {
                using var sha256 = SHA256.Create();
                var bytes = Encoding.UTF8.GetBytes(password);
                var hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
            catch (Exception ex)
            {
                throw new Exception("Error hashing password", ex);
            }
        }

        private bool VerifyPassword(string password, string storedHash)
        {
            // Add proper null checking
            if (string.IsNullOrEmpty(password) || string.IsNullOrEmpty(storedHash))
                return false;

            try
            {
                var hash = HashPassword(password);
                return hash == storedHash;
            }
            catch
            {
                return false;
            }
        }



    }
}