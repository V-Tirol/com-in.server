namespace com_in.server.DTO
{
    public class TextMediaUploadDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateOnly Date { get; set; }
        public string? Description { get; set; } = "";

        public string fileType { get; set; }
        public int uploaderId { get; set; }
        public string category { get; set; }
        public string categoryType { get; set; } //student , faculty

        public IFormFile uploadedFile { get; set; }

        public bool isApproved { get; set; } // 0 by default, 1 if superadmin
    }
}
