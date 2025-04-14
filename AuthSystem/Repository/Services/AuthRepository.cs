using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Security.Cryptography;
using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Models;
using AuthSystem.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using StackExchange.Redis;
namespace AuthSystem.Repository.Services
{

    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        private readonly IConnectionMultiplexer _redis;
        public AuthRepository(ApplicationDbContext context, IConfiguration config,IConnectionMultiplexer redis)
        {
            _context = context;
            _config = config;
            _redis = redis;
        }
        public async Task<UserModel> GetUserAsync(int id)
        {
            return await _context.Users.FindAsync(id);
        }
        public async Task<string>LogoutAsync(string userId)
        {
            var db = _redis.GetDatabase();
            await db.KeyDeleteAsync(userId);
            return "Logged out";
        }
        public async Task<(UserModel, string)>Login(LoginDTO userData)
        {
            var user =  await Authenticate(userData);
            if (user != null)
            {
                var token = GenerateToken(user);
                user.Password = "";
                var db = _redis.GetDatabase();
                var userId = user.Id.ToString();
                var existingSession = await db.StringGetAsync(userId);
                if (!string.IsNullOrEmpty(existingSession))
                {
                    await db.KeyDeleteAsync(userId);
                }
                await db.StringSetAsync(userId, token, TimeSpan.FromMinutes(30));
                return (user, token);
            }
            return (null, null);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
        public async Task<UserModel> SignupAsync(UserModel user)
        {
            string password = PasswordHasher(user.Password);
            user.Password = password;
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            var profile = new ProfileModel
            {
                userId = user.Id,
                MobileNumber = string.Empty,
                Gender = string.Empty,
                User = user
            };
            _context.Profiles.Add(profile);
            _context.SaveChanges();
            return user;
        }
        public async Task<bool>UserExistsAsync(string usrname)
        {
            return await _context.Users.AnyAsync(o => o.UserName.ToLower() == usrname.ToLower());
        }

        public async Task<bool>EmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(o => o.EmailAddress.ToLower() == email.ToLower());
        }
        public async Task<bool> MatchPassword(string userName, string password)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName.ToLower() == userName.ToLower());
            var hashedPassword = user.Password;
            var currentHash = PasswordHasher(password);
            if (hashedPassword != currentHash)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public async Task<UserModel> Authenticate(LoginDTO userData)
        {
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.UserName.ToLower() == userData.Username.ToLower());
            if (currentUser == null)
            {
                return null;
            }
            var isValid = await MatchPassword(userData.Username, userData.Password);
            if(isValid)
            {
                return currentUser;
            }
            else
            {
                return null;
            }
        }
        public string PasswordHasher(string password)
        {
            const int SaltSize = 16;
            const int HashSize = 32;
            const int Iterations = 1000;
            HashAlgorithmName Algorithm = HashAlgorithmName.SHA512;

            //byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] salt = new byte[]
                        {
                        159, 12, 126, 82, 149, 167, 163, 78,
                         31, 94, 43, 19, 168, 205, 27, 227
                        };
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algorithm, HashSize);
            return $"{Convert.ToHexString(hash)}";
        }
        public string GenerateToken(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.EmailAddress),
                new Claim(ClaimTypes.Role, user.Role),
            };
            var token = new JwtSecurityToken(
                _config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
