namespace com_in.server.Models
{
    public class Admin
    {
        public int Id { get; set; }
        public string InstitutionalEmail { set; get; } 
        public string Name {  get; set; }
        public string Position {  set; get; }
        public bool isActive {  set; get; }
    }
}
