namespace com_in.server.Models
{
    public class Faculty
    {
        public int Id { get; set; }
        public string FacultyId {  get; set; }
        public string InstitutionalEmail {  get; set; }
        public int DepartmentId {  get; set; }
        public string Name {  get; set; }
        public string Position { get; set; }
        public bool isActive {  get; set; }
    }
}
