using System.Text.Json.Serialization;

namespace com_in.server.Models
{
    public class Article
    {
        public int Id { get; set; }
        public string Title { get; set; }  
        public string Author { get; set; } 
        public DateOnly Date {  get; set; }
        public bool IsActive { get; set; }
        public int CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category? Category { get; set; }
    }
}
