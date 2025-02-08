using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Models
{
    public class ProfileModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public int userId { get; set; }
        public string MobileNumber { get; set; } = String.Empty;
        public string Gender { get; set; } = String.Empty;
        [JsonIgnore]
        public UserModel User { get; set; } = null!;
    }
}