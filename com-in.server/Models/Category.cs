using System.Text.Json.Serialization;

namespace com_in.server.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string CategoryName {  get; set; }
        public bool isActive { get; set; }
        [JsonIgnore]
        public virtual List<Article> Articles { get; set; }
        [JsonIgnore]
        public virtual List<Media> Media { get; set; }
    }
}
