using System.ComponentModel.DataAnnotations;

namespace TMSBlazorAPI.Models.User
{
    public class UserCreateDto
    {
        [Required]
        [StringLength(25)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(25)]  
        public string LastName { get; set; }
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public string Username { get; set; }
    }
}
