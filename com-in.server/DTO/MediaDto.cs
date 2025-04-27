using com_in.server.Models;

namespace com_in.server.DTO
{
    public class MediaDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Duration { get; set; }
        public string Views { get; set; }
        public bool isActive { get; set; }
        public string Type { get; set; }
        public string Category { get; set; }
    }
}
