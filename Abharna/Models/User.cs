using System.ComponentModel.DataAnnotations;

namespace Abharna.Models
{
    public class User
    {
        public int Id { get; set; }

        [Required, StringLength(40)]
        public string FullName { get; set; } = "";

        [Required, StringLength(100)]
        public string Email { get; set; } = "";

        [Required, StringLength(10)]
        public string Phone { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";

        [Required]
        public string AccountType { get; set; } = "";

        public string ContactBy { get; set; } = "Email";
    }
}
