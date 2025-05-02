using inventory_app_backend.Constants;
using inventory_app_backend.DTO.User;
using inventory_app_backend.Models;
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
        Task<List<GetUserDTO>> AllUsers();
        Task<int> AddUser(UserDTO user);
        Task<int> UpdateUser(UpdateUserDTO user);
        Task<int> DeleteUser(int id);
        Task<User> GetUserByEmail(string email);
        Task<LoginResultDTO> GetUserByEmailAndPassword(string email, string password);
        bool VerifyPassword(string enteredPassword, string storedHash);
        string GenerateToken(string email);
        Task<UserDTO> GetUser(int id);
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

        public async Task<int> AddUser(UserDTO userDTO)
        {
            var user = new User
            {
                Name = userDTO.Name,
                Email = userDTO.Email,
                Password = HashPassword(userDTO.Password),
                IdUserRole = userDTO.IdRole,
                IdStatus = (int)Status.Active
            };
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

        public async Task<List<GetUserDTO>> AllUsers()
        {
            try
            {
                var users = await _context.Users
                    .Where(o => o.IdStatus == (int)Status.Active)
                    .Select(u => new
                    {
                        u.IdUser,
                        u.Name,
                        u.Email,
                        u.IdUserRole
                    })
                    .ToListAsync();

                var result = users.Select(u => new GetUserDTO
                {
                    IdUser = u.IdUser,
                    Name = u.Name,
                    Email = u.Email,
                    IdRole = u.IdUserRole,
                    RoleName = Enum.GetName(typeof(Roles), u.IdUserRole) ?? "-"
                }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all users", ex);
            }
        }

        public async Task<int> DeleteUser(int id)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(id);
                if (existingUser == null)
                {
                    throw new Exception("User not found");
                }
                existingUser.IdStatus = (int)Status.Inactive;
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user", ex);
            }
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
            
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                return user;
            }
            throw new Exception("User not found");
        }

        public async Task<LoginResultDTO> GetUserByEmailAndPassword(string email, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email && u.Password == password);
            if (user == null)
            {
                return null;
            }
            return new LoginResultDTO
            {
                IdUser = user.IdUser,
                Name = user.Name,
                Email = user.Email,
                IdUserRole = user.IdUserRole,
                UserRoleName = Enum.GetName(typeof(Roles), user.IdUserRole) ?? "-"
            };
        }

        public async Task<int> UpdateUser(UpdateUserDTO user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(user.IdUser);
                if (existingUser == null)
                {
                    throw new Exception("User not found");
                }
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.IdUserRole = user.IdRole;
                existingUser.Password = HashPassword(user.Password);
                return await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while updating the user", ex);
            }
        }

        public async Task<UserDTO> GetUser(int id)
        {
            try
            {
                var user = await _context.Users
                    .Where(u => u.IdUser == id)
                    .Select(u => new UserDTO
                    {
                        Name = u.Name,
                        Email = u.Email,
                        IdRole = u.IdUserRole
                    })
                    .FirstOrDefaultAsync();
                if (user == null)
                {
                    throw new Exception("User not found");
                }
                return user;
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the user", ex);
            }
        }
    }
}
