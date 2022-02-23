namespace API.Models.User
{
    public class UpdatePassword
    {
        public string Password { get; set; }

        public string ConfirmPassword { get; set; }
    }
}