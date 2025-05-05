using System.Text.Json.Serialization;

namespace com_in.server.Models
{
    public class Course
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool isDeleted { get; set; }


        [JsonIgnore]
        public List<Student> student { get; set; }
    }
}
