using System.ComponentModel.DataAnnotations;

namespace API.Models
{
    public class Account
    {
        [Key]
        public string UserId { get; set; }
        public string Password { get; set; }
        public int OTP { get; set; }
        public bool IsUsed { get; set; }
        public string Role { get; set; }
        public virtual User User { get; set; }
    }
}
