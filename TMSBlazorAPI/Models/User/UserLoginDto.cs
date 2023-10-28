using System.ComponentModel.DataAnnotations;

namespace TMSBlazorAPI.Models.User
{
    public class UserLoginDto
    {
        [Required]
        public string Username { get; set; }
        [Required]
        public string Password { get; set; }

        


        
    }
}
