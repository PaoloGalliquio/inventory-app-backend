using inventory_app_backend.Mapppers;
using inventory_app_backend.Models;
using inventory_app_backend.ViewModels.User;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace inventory_app_backend.Services
{
    public interface IUserService
    {
        Task<List<User>> AllUsers();
        Task<int> AddUser(CreateUserViewModel user);
        Task<int> UpdateUser(User user);
        Task<int> DeleteUser(int id);
        Task<User> GetUserById(int id);
        Task<User> GetUserByEmail(string email);
        Task<UserViewModel> GetUserByEmailAndPassword(string email, string password);
        bool VerifyPassword(string enteredPassword, string storedHash);
        string GenerateToken(string email);
    }

    public class UserService : IUserService
    {
        private readonly InventoryContext _context;
        private readonly DbSet<User> _dbSet;
        private readonly IConfiguration _configurationManager;

        public UserService(InventoryContext context, IConfiguration configurationManager)
        {
            _context = context;
            _dbSet = context.Set<User>();
            _configurationManager = configurationManager;
        }

        public async Task<int> AddUser(CreateUserViewModel viewModel)
        {
            var user = UserMapper.MapCreateUserViewModelToUser(viewModel);
            user.Password = HashPassword(viewModel.Password);
            await _context.Users.AddAsync(user);
            int afectedRow = await _context.SaveChangesAsync();
            return afectedRow;
        }

        private static string HashPassword(string password)
        {
            int iterations = 10000;
            int saltSize = 16;
            int hashSize = 32;

            byte[] salt = new byte[saltSize];
            RandomNumberGenerator.Fill(salt);

            using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(hashSize);

            byte[] hashBytes = new byte[saltSize + hashSize];
            Array.Copy(salt, 0, hashBytes, 0, saltSize);
            Array.Copy(hash, 0, hashBytes, saltSize, hashSize);

            string hashedPassword = Convert.ToBase64String(hashBytes);

            return hashedPassword;
        }

        public bool VerifyPassword(string enteredPassword, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            int saltSize = 16;
            byte[] salt = new byte[saltSize];
            Array.Copy(hashBytes, 0, salt, 0, saltSize);

            int iterations = 10000;
            int hashSize = 32;
            using var pbkdf2 = new Rfc2898DeriveBytes(enteredPassword, salt, iterations, HashAlgorithmName.SHA256);
            byte[] hash = pbkdf2.GetBytes(hashSize);

            for (int i = 0; i < hashSize; i++)
            {
                if (hashBytes[i + saltSize] != hash[i])
                {
                    return false;
                }
            }
            return true;
        }

        public async Task<List<User>> AllUsers()
        {
            return await _dbSet.ToListAsync();
        }

        public async Task<int> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return 0;
            }
            _context.Users.Remove(user);
            return await _context.SaveChangesAsync();
        }

        public string GenerateToken(string email)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configurationManager["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _configurationManager["Jwt:Issuer"],
                audience: _configurationManager["Jwt:Issuer"],
                claims: claims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<UserViewModel> GetUserByEmailAndPassword(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                return null;
            }
            return UserMapper.MapTopUserViewModel(user);
        }

        public Task<User> GetUserById(int id)
        {
            return _context.Users.FirstOrDefaultAsync(u => u.IdUser == id);
        }

        public async Task<int> UpdateUser(User user)
        {
            _context.Users.Update(user);
            return await _context.SaveChangesAsync();
        }
    }
}
