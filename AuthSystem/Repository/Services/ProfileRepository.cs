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
        public async Task<ICollection<AddressDTO>>FindAddress(int userId)
        {
            var result = await _context.Addressess.Where((i)=>i.UserId == userId).ToListAsync();
            var addressDtos = result.Select(a => new AddressDTO
            {
                AddressLine = a.AddressLine,
                City = a.City,
                State = a.State,
                PinCode = a.PinCode,
                Country = a.Country
            }).ToList();
            return addressDtos;
        }
        public async Task<AddressModel> AddAddress(AddressDTO address,int userId)
        {
            var addressModel = new AddressModel
            {
                AddressLine = address.AddressLine,
                City = address.City,
                State = address.State,
                PinCode = address.PinCode,
                Country = address.Country,
                UserId = userId
            };
            await _context.Addressess.AddAsync(addressModel);
            _context.SaveChanges();
            return addressModel;
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
