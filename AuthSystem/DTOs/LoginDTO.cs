using System.ComponentModel.DataAnnotations;

namespace AuthSystem.DTOs
{
    public class LoginDTO
    {
        [Required(ErrorMessage = "Email is Required")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Password is Required")]
        public string Password { get; set; }

    }
}
