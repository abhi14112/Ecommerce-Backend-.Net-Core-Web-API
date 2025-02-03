using AuthSystem.DTOs;
using AuthSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Repository.Interface
{
    public interface IAuthRepository
    {
        Task<IActionResult> Logout();
        Task<IActionResult> Login(LoginDTO userData);
        Task<IActionResult> Signup(UserModel user);
    }
}
