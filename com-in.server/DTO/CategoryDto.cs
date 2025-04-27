namespace com_in.server.DTO
{
    public class CategoryDto
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public bool isActive { get; set; }
        public List<ArticleDto> Articles { get; set; }
    }
}
