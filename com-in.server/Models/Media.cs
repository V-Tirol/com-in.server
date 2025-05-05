using System.Text.Json.Serialization;

namespace com_in.server.Models
{
    public class Media
    {
        public int Id {  get; set; }  
        public string Title { get; set; }
        public string Duration { get; set; }
        public string Views {  get; set; }
        public string filter {  get; set; }
        public bool isActive {  get; set; }

        public int TypeId { get; set; }
        [JsonIgnore]
        public MediaType Type { get; set; }

        public int CategoryId { get; set; }
        [JsonIgnore]
        public virtual Category? Category { get; set; }
    }
}
