namespace com_in.server.DTO
{
    public class AllAccountDto
    {

    }

    public class OrgFac
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string userType { get; set; }
        public string ProfileId { get; set; }
        public string Email {  get; set; }
        public string department {  get; set; }
        public string position { get; set; }

        public int iterator { get; set; }
    }

    public class StudAlum
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string userType { get; set; }
        public string ProfileId { get; set; }
        public string Email { get; set; }


        public string? currentPosition { get; set; }
        public string? graduationYear { get; set; }
        public string? course {  get; set; }

        public int loginId { get; set; }

        public int iterator { get; set; }
        public bool isActive { get; set; }
    }
}
