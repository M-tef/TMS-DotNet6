using System.ComponentModel.DataAnnotations;

namespace TMSBlazorAPI.Models.Club
{
    public class ClubCreateDto
    {
        [Required]
        [StringLength(50)]
        public string ClubName { get; set; }
        [Required]
        public int UserID{ get; set; }

    }
}
