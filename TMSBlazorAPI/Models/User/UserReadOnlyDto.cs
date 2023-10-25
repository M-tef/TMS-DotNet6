namespace TMSBlazorAPI.Models.User
{
    public class UserReadOnlyDto : BaseDto
    {
        //public string CreatedBy { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        //public string Phone { get; set; }
    }
}
