namespace com_in.server.Models
{
    public class Student
    {
        public int Id { get; set; }
        public string StudentId {  get; set; }
        public string Name {  get; set; }
        public int courseId { get; set; }
        public Course course {  get; set; }
        public string Email { get; set; }
        public bool isActive { get; set; }
    }
}
