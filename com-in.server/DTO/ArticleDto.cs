namespace com_in.server.DTO
{
    public class ArticleDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public DateOnly Date { get; set; }
        public bool IsActive { get; set; }

        public string category { get; set; } // student, faculty, org, research extension
        public string filter { get; set; } // courses, department
    }
}
