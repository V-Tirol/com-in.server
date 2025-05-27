namespace com_in.server.DTO
{
    public class AnnouncementInsertDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Category { get; set; }
        public int PublishedById { get; set; }
        public string Status { get; set; }
        public IFormFile? image { get; set; }
    }

    public class AnnouncementReturnDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Excerpt { get; set; }
        public string Category { get; set; }
        public int PublishedById { get; set; }
        public string Status { get; set; }
        public string imageUrl { get; set; }
        public DateTime? PublishDateAndTime { get; set; }
    }
}
