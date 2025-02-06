
using AuthSystem.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthSystem.Mappers;
namespace AuthSystem.Controllers
{
    [Route("api/[controller]")]
    public class ProfileController:ControllerBase
    {
        private readonly IProfileRepository _profileService;
        private readonly IAuthRepository _authService;
        public ProfileController(IProfileRepository profileRepository,IAuthRepository authRepository) 
        { 
            _profileService = profileRepository;
            _authService = authRepository;
        }
        [HttpGet("{id}")]
        //[Authorize]
        public async Task<IActionResult>GetProfileData(int id)
        {
            var profile = await _profileService.GetProfileById(id);
            var user = await _authService.GetUserAsync(id);
            var userProfileData = ProfileMapper.ToProfileDto(profile, user);
            return Ok(userProfileData); 
        }
    }
}
