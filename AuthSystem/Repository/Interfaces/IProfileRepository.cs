using AuthSystem.DTOs;
using AuthSystem.Models;
namespace AuthSystem.Repository.Interface
{
    public interface IProfileRepository
    {
        Task<ProfileModel>GetProfileById(int id);
        Task<bool> PhoneExistsAsync(string phone,int id);
        Task SaveChangesAsync();
        Task<AddressModel> AddAddress(AddressDTO address, int userId);  
        Task<ICollection<AddressDTO>> FindAddress(int userId);
        Task<bool> CheckEmailExistsAsync(string email,int id);
        Task DeleteAddressAsync(int id);
    }

}
