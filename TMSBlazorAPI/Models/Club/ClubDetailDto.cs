using System.ComponentModel.DataAnnotations;

namespace TMSBlazorAPI.Models.Club
{
    public class ClubDetailDto: BaseDto
    {
        [Required]
        [StringLength(25)]
        public string ClubName { get; set; }
        [Required]
        [StringLength(25)]
        public string Users { get; set; }
        [Required]
        public int UserID { get; set; }
    }
}
