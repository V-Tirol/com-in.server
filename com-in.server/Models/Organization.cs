namespace com_in.server.Models
{
    public class Organization
    {
        public int Id { get; set; }
        public string OrganizationId {  get; set; }
        public string OrganizationName { get; set; }
        public string OrganizationEmail {  get; set; }
        public bool isActive {  get; set; }
    }
}
