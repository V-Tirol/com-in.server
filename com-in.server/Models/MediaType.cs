using System.Text.Json.Serialization;

namespace com_in.server.Models
{
    public class MediaType
    {
        public int Id {  get; set; }
        public string Name { get; set; }
        public bool isActive {  get; set; }
        [JsonIgnore]
        public List<Media> media { get; set; }
    }
}
