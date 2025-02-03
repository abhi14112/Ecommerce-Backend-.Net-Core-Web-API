using AuthSystem.DTOs;
using AuthSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Repository.Interface
{
    public interface IAuthRepository
    {
        Task<IActionResult> Logout();
        Task<(UserModel, string)> Login(LoginDTO userData);
        Task<IActionResult> Signup(UserModel user);
        Task<UserModel> Authenticate(LoginDTO userData);
        string GenerateToken(UserModel user);
    }
}
