
using AuthSystem.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using AuthSystem.Mappers;
using AuthSystem.DTOs;
using AuthSystem.Models;
using System.Security.Claims;
using Stripe;
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
        
        [HttpGet("GetAddress")]
        [Authorize]
        public async Task<IActionResult>GetAddress()
        {
            var userId1 = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            int userId = Convert.ToInt32(userId1);
            var res = await _profileService.FindAddress(userId);
            return Ok(res);
        }
        [HttpPost("CreateAddress")]
        [Authorize]
        public async Task<IActionResult>CreateAddress([FromBody]AddressDTO address)
        {
            var userId1 = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
           int userId = Convert.ToInt32(userId1);
            var res = await _profileService.AddAddress(address,userId);
            return Ok(res);
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
        [HttpPut("{id}")]
        public async Task<IActionResult>UpdateProfile(int id, [FromBody]ProfileDTO profileData)
        {
            //return Ok(profileData);
            try
            {
                var profile = await _profileService.GetProfileById(id);
                var user = await _authService.GetUserAsync(id);
                if (profile == null)
                {
                    return NotFound(new { Message = "Product not found" });
                }
                if (!String.IsNullOrEmpty(profileData.firstName)) 
                    user.FirstName = profileData.firstName;
                if(!String.IsNullOrEmpty(profileData.lastName))
                    user.LastName = profileData.lastName;
                if(!String.IsNullOrEmpty(profileData.email))
                {
                    
                    bool isEmail = await _authService.EmailExistsAsync(profileData.email);
                    if (isEmail)
                    {
                        return BadRequest(new { message = "Email already exists" });

                    }
                    user.EmailAddress = profileData.email;

                }
                    
                if(!string.IsNullOrEmpty(profileData.phone))
                {
                    bool isPhone = await _profileService.PhoneExistsAsync(profileData.phone);
                    if (isPhone)
                        return BadRequest(new { message = "Phone already Exists" });

                    profile.MobileNumber = profileData.phone;
                }
                if (!string.IsNullOrEmpty(profileData.gender))
                    profile.Gender = profileData.gender;
                await _authService.SaveChangesAsync();
                await _profileService.SaveChangesAsync();
                return Ok(ProfileMapper.ToProfileDto(profile, user));
            }
            catch (Exception ex)
            { 
                return BadRequest(ex.Message);
            }

        }
    }
}
