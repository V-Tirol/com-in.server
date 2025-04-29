namespace com_in.server.Models
{
    public class Alumni
    {
        public int Id {  get; set; }
        public string Email {  get; set; }
        public string Name { get; set; }
        public string StudentId { get; set; }
        public string YearGraduated { get; set; }
        public string? CurrentPosition { get; set; }
        public bool isActive {  get; set; }

    }
}
