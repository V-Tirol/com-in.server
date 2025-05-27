namespace com_in.server.Models
{
    public class Announcements
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string Category {  get; set; }
        public DateTime PublishDateAndTime { get; set; }
        public int PublishedById { get; set; }

        public string Status { get; set; } 
        public string imageURL { get; set; }


    }
}
