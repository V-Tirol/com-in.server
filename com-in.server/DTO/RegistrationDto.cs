namespace com_in.server.DTO
{
    public class RegistrationDto
    {
        public string Email {  get; set; }
        public string Password { get; set; }
        public string FullName {  get; set; }
        public string UserType {  get; set; }
        public string ProfileId {  get; set; }  //StudentId, FacultyId, OrganizationalId


        public string? course { get; set; } //Student Course
        public string Position { get; set; } = ""; //Alumni,Faculty Position
        public string? department { get; set; }//Faculty Department

        public string? YearGraduated { get; set; } = "";

        public bool isAddedBySupOrAdmin { get; set; } = false;
    }

    public class BaseUserDto // pending
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        public string UserType { get; set; }
        public string ProfileId { get; set; }
    }

    public class FacultyDto : BaseUserDto
    {
        public string department { get; set; }
        public string? position {  get; set; }
    }

    public class OrgDto : BaseUserDto
    {

    }

}
