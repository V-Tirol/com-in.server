using com_in.server.Models;

namespace com_in.server.DTO
{
    public class UserInfoDto
    {
        public Student student { get; set; }
        public Admin admin { get; set; }
        public Faculty faculty { get; set; }
        public Alumni alumni { get; set; }
        public Organization organization { get; set; }
    }
}
