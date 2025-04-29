using com_in.server.Models;

namespace com_in.server.DTO
{
    public class UserInfoDto
    {
        //public string Email { get; set; }
        //public string Password { get; set; }
        //public string FullName { get; set; }
        //public string UserType { get; set; }
        //public string ProfileId { get; set; }  //StudentId, FacultyId, OrganizationalId


        //public string Course { get; set; }//Student Course
        //public string Position { get; set; } //Alumni,Faculty Position
        //public string Department { get; set; }//Faculty Department

        //public DateOnly YearGraduated { get; set; }

        //public int LoginId { get; set; }
        //public Login Login { get; set; }

        //// Properties for Student
        //public int? StudentID { get; set; } // Nullable, because it may not exist for all users
        //public string StudentFullName { get; set; }
        //public string StudentCourse { get; set; }
        //public string StudentEmail { get; set; }

        //// Properties for Faculty
        //public string FacultyInstitutionalEmail { get; set; }
        //public int? FacultyID { get; set; } // Nullable
        //public string FacultyDepartment { get; set; }
        //public string FacultyPosition { get; set; }

        //// Properties for Alumni
        //public int? GraduationYear { get; set; } // Nullable, because it may not exist for all users
        //public string AlumniCurrentPosition { get; set; }

        //// Properties for Organization
        //public int? OrganizationId { get; set; } // Nullable
        //public string OrganizationName { get; set; }
        //public string OrganizationEmail { get; set; }

        //// Properties for Admin
        //public string AdminInstitutionalEmail { get; set; }
        //public string AdminPosition { get; set; }

        public Student student { get; set; }
        public Admin admin { get; set; }
        public Faculty faculty { get; set; }
        public Alumni alumni { get; set; }
        public Organization organization { get; set; }
    }
}
