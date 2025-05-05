namespace com_in.server.DTO
{
    public class RegistrationDto
    {
        public string Email {  get; set; }
        public string Password { get; set; }
        public string FullName {  get; set; }
        public string UserType {  get; set; }
        public string ProfileId {  get; set; }  //StudentId, FacultyId, OrganizationalId


        public int courseId { get; set; } //Student Course
        public string Position { get; set; } = ""; //Alumni,Faculty Position
        public int departmentId { get; set; }//Faculty Department

        public string YearGraduated { get; set; }
    }
}
