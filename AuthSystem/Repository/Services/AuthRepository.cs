using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Models;
using AuthSystem.Repository.Interface;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Repository.Services
{
    public class AuthRepository : IAuthRepository
    {
        private readonly ApplicationDbContext _context;
        public AuthRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        public Task<IActionResult>Logout()
        {
            throw new NotImplementedException();
        }
        public Task<IActionResult>Login(LoginDTO userData)
        {
            throw new NotImplementedException();
        }
        public Task<IActionResult> Signup(UserModel user)
        {
            throw new NotImplementedException();
        }
    }
}
