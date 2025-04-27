using System.Text.Json.Serialization;

namespace com_in.server.Models
{
    public class Media
    {
        public int Id {  get; set; }  
        public string Title { get; set; }
        
        public int TypeId { get; set; }
        
        public string Duration { get; set; }
        public string Views {  get; set; }
        public int CategoryId {  get; set; }
        public bool isActive {  get; set; }

        [JsonIgnore]
        public MediaType Type { get; set; }
        [JsonIgnore]
        public Category Category { get; set; }
    }
}
