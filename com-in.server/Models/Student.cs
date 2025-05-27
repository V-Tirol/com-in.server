namespace com_in.server.Models
{
    public class Student
    {
        public int Id { get; set; }
        public int StudentId {  get; set; }
        public string Name {  get; set; }
        public string course { get; set; }
        public string Email { get; set; }
        public bool isActive { get; set; }
    }
}
