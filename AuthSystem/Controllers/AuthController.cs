using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthSystem.Data;
using AuthSystem.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace AuthSystem.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private IConfiguration _config;
        private readonly ApplicationDbContext _context;
        public AuthController(IConfiguration config ,ApplicationDbContext context)
        {
            _context = context;
            _config = config;
        }
        [AllowAnonymous]
        [HttpPost("Login")]
        public IActionResult Login([FromBody] UserLogin userLogin)
        {
            var user = Authenticate(userLogin);
            if(user != null)
            {
                var token = Generate(user);
                return Ok(token);
            }
            return NotFound("User not found");
        }

        [AllowAnonymous]
        [HttpPost("Signup")]
        public IActionResult Signup([FromBody] UserModel user)
        {
            if (user == null)
            {
                return BadRequest("User Data is Required");
            }
            if(_context.Users.Any(o => o.UserName.ToLower() == user.UserName.ToLower()))
            {
                return BadRequest("User Name already exists");
            }
            if(_context.Users.Any(o => o.EmailAddress.ToLower() == user.EmailAddress.ToLower()))
            {
                return BadRequest("Email Address already exists");
            }
            try
            {
                var token = Generate(user);
                _context.Users.Add(user);
                _context.SaveChanges();
                return Ok(token);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error has occured whle adding the user: {ex.Message}");
            }
        }

        private string Generate(UserModel user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));

            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
           
                new Claim(ClaimTypes.NameIdentifier, user.UserName),
                new Claim(ClaimTypes.Email,user.EmailAddress),
                new Claim(ClaimTypes.Role,user.Role),

            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: credentials
                );

            return new JwtSecurityTokenHandler().WriteToken(token);
                            
        }

        private UserModel Authenticate(UserLogin userLogin)
        {
            var currentUser = _context.Users.FirstOrDefault( o => o.UserName.ToLower() == userLogin.Username.ToLower() && o.Password == userLogin.Password);

            if (currentUser != null)
            {
                return currentUser;
            }
            return null;
        }
    }
}
