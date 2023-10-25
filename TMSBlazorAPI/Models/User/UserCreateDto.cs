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
    }
}
