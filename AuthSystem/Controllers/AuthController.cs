using AuthSystem.DTOs;
using AuthSystem.Models;
using AuthSystem.Repository.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/[controller]")]
[ApiController]
public class AuthController : ControllerBase
{
    private IConfiguration _config;
    public readonly IAuthRepository _authService;
    public AuthController(IConfiguration config, IAuthRepository authService)
    {
        _config = config;
        _authService = authService;
    }

    [HttpPost("Logout")]
    [Authorize]
    public IActionResult Logout()
    {
        return Ok(new { message = "Logged out successfully" });
    }

    [AllowAnonymous]
    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO userData)
    {
        var (user, token) = await _authService.Login(userData);
        if(user!=null)
        {
            user.Password = "";
            return Ok(new
            {
                message = "Login successful",
                token = token,
                user = user
            });
        }
        return NotFound("User not found");
    }
    [AllowAnonymous]
    [HttpPost("Signup")]
    public async Task<IActionResult> Signup([FromBody] UserModel user)
    {
        if(user == null)
        {
            return BadRequest(new { message = "User Data is required" });
        }
        if(await _authService.EmailExistsAsync(user.EmailAddress))
        {
            return BadRequest(new { message = "Email Already Exists" });
        }
        try
        {
            var newUser = await _authService.SignupAsync(user);
            var token = _authService.GenerateToken(newUser);
            newUser.Password = "";
            return Ok();
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred during signup." });
        }
    }
}