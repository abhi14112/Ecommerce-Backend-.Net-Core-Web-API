using AuthSystem.Data;
using AuthSystem.DTOs;
using AuthSystem.Models;
using AuthSystem.Repository.Interface;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Repository.Services
{
    public class ProfileRepository:IProfileRepository
    {
        private readonly ApplicationDbContext _context;
        public ProfileRepository(ApplicationDbContext context) 
        {
            _context = context;
        }
        public async Task<ProfileModel> GetProfileById(int id)
        {
            var profile = await _context.Profiles.FindAsync(id);
            return profile;
        }
        public async Task<bool> PhoneExistsAsync(string phone)
        {
            return await _context.Profiles.AnyAsync(o => o.MobileNumber == phone);
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
