namespace com_in.server.Models
{
    public class MultiMedia
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateOnly Date { get; set; }
        public string? Description { get; set; }
        public TimeSpan? duration { get; set; }
        public string fileURL { get; set; }

        public string mediaType { get; set; }
        public int uploaderId { get; set; }
        public string departmentName { get; set; }
        public string departmentType { get; set; } //student , faculty
        public bool isSuperAdminApprove { get; set; }
        public bool isDeleted { get; set; }
    }
}
