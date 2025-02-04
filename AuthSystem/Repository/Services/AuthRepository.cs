﻿using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Models;
using AuthSystem.Repository.Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
namespace AuthSystem.Repository.Services
{

    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _config;
        public AuthRepository(ApplicationDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public async Task<(UserModel, string)>Login(LoginDTO userData)
        {
            var user =  await Authenticate(userData);
            if (user != null)
            {
                var token = GenerateToken(user);
                user.Password = "";
                return (user, token);
            }
            return (null, null);
        }
        public async Task<UserModel> SignupAsync(UserModel user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
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

        public async Task<UserModel> Authenticate(LoginDTO userData)
        {
            var currentUser = await _context.Users
                .FirstOrDefaultAsync(u =>
                    u.UserName.ToLower() == userData.Username.ToLower() &&
                    u.Password == userData.Password);

            return currentUser;
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
