using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace AuthSystem.Models
{
    public enum GenderEnum
    {
        Male,
        Female
    }
    public class ProfileModel
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("UserId")]
        public int userId { get; set; }
        public string MobileNumber { get; set; } = String.Empty;
        public GenderEnum? Gender { get; set; }
        [JsonIgnore]
        public UserModel User { get; set; } = null!;
    }
}