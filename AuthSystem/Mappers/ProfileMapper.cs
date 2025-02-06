using AuthSystem.DTOs;
using AuthSystem.Models;

namespace AuthSystem.Mappers
{
    public static class ProfileMapper
    {
        public static ProfileDTO ToProfileDto(ProfileModel profile, UserModel user)
        {
            var profileDto = new ProfileDTO()
            {
                firstName = user.FirstName,
                lastName = user.LastName,
                email = user.EmailAddress,
                phone = profile.MobileNumber,
                gender = profile.Gender
            };
            return profileDto;
        }
    }
}
