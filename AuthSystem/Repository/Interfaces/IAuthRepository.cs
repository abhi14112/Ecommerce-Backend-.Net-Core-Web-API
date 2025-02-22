﻿using AuthSystem.DTOs;
using AuthSystem.Models;
using Microsoft.AspNetCore.Mvc;

namespace AuthSystem.Repository.Interface
{
    public interface IAuthRepository
    {
        Task<(UserModel, string)> Login(LoginDTO userData);
        Task<UserModel> SignupAsync(UserModel user);
        Task<String>LogoutAsync(string userId);
        Task<UserModel> GetUserAsync(int id);
        Task<bool> UserExistsAsync(string username);
        Task<bool>EmailExistsAsync(string email);
        Task<UserModel> Authenticate(LoginDTO userData);
        Task SaveChangesAsync();
        string GenerateToken(UserModel user);
    }
}