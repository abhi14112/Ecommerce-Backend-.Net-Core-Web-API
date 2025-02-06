using AuthSystem.Models;
namespace AuthSystem.Repository.Interface
{
    public interface IProfileRepository
    {
        Task<ProfileModel>GetProfileById(int id);
    }
}
