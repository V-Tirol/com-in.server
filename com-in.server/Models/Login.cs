namespace com_in.server.Models
{
    public class Login
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string UserType {  get; set; }
        public int ReferenceId {  get; set; }
        public DateOnly LastLoginDate { get; set; }
        public TimeOnly LastLoginTime { get; set; }
        public bool isActive { get; set; }

        public bool isEmailConfirmed {  get; set; }
        public string? EmailConfirmationToken { get; set; }
        public DateTime? EmailConfirmationTokenExpiry { get; set; }

    }
}
