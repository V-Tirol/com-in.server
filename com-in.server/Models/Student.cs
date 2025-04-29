namespace com_in.server.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string StudentId {  get; set; }
        public string Name {  get; set; }
        public string Course {  get; set; }
        public string Email { get; set; }
        public bool isActive { get; set; }
    }
}
