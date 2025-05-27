using System.Text.Json.Serialization;

namespace com_in.server.Models
{
    public class Category
    {
        public int Id { get; set; }
        public string Name {  get; set; }
        public bool isActive { get; set; }

    }
}
