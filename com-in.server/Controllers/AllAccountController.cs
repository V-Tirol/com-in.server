using com_in.server.DTO;
using com_in.server.Hasher;
using com_in.server.Models;
using com_in.server.Service;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace com_in.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AllAccountController : ControllerBase
    {
        private ForumContext _context;
        private readonly IPasswordHasher _passwordHasher;
        public AllAccountController(ForumContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        [HttpGet("organization-faculty")]
        public async Task<ActionResult<OrgFac>> getOrgFac()
        {
            var fac = await _context.Faculties
                .Select(item => new OrgFac
                {
                    Id = item.Id,
                    Name = item.Name,
                    department = item.Department,
                    Email = item.InstitutionalEmail,
                    position = item.Position,
                    ProfileId = item.FacultyId.ToString(),
                    userType = "faculty"
                }).ToListAsync();

            var facWithIterator = fac
                .Select((item, index) => new OrgFac
                {
                    Id = item.Id,
                    Name = item.Name,
                    department = item.department,
                    Email = item.Email,
                    position = item.position,
                    ProfileId = item.ProfileId,
                    userType = item.userType,
                    iterator = index + 1
                });

            var org = await _context.Organizations
                .Select(item => new OrgFac
                {
                    Id = item.Id,
                    Name = item.OrganizationName,
                    Email = item.OrganizationEmail,
                    userType = "organization",
                    ProfileId = item.OrganizationId
                }).ToListAsync();

            var orgWithIterator = org
                .Select((item, index) => new OrgFac
                {
                    Id = item.Id,
                    Name = item.Name,
                    Email = item.Email,
                    userType = item.userType,
                    ProfileId = item.ProfileId,
                    iterator = index + 1 + fac.Count()
                });

            var combined = facWithIterator.Concat(orgWithIterator).ToList();
            return Ok(combined);
        }

        [HttpGet("students-alumni")]
        public async Task<ActionResult<IEnumerable<StudAlum>>> getStudents()
        {
            #region commented
            //var students = await _context.Students
            //    .Select(item => new StudAlum
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        userType = "student",
            //        ProfileId = item.StudentId.ToString(),
            //        Email = item.Email,
            //        course = item.course,
            //        isActive = item.isActive

            //    })
            //    .ToListAsync();

            //var alumni = await _context.Alumni
            //    .Select(item => new StudAlum
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        userType = "alumni",
            //        ProfileId = item.StudentId.ToString(),
            //        Email = item.Email,
            //        graduationYear = item.YearGraduated,
            //        currentPosition = item.CurrentPosition,
            //        isActive = item.isActive
            //    }).ToListAsync();

            //var studentsWithIterator = students
            //    .Select((item, index) => new StudAlum
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        userType = "student",
            //        ProfileId = item.ProfileId,
            //        Email = item.Email,
            //        course = item.course,
            //        isActive = item.isActive,
            //        iterator = index + 1

            //    });

            //var alumniWithIterator = alumni
            //    .Select((item, index) => new StudAlum
            //    {
            //        Id = item.Id,
            //        Name = item.Name,
            //        userType = "alumni",
            //        ProfileId = item.ProfileId,
            //        Email = item.Email,
            //        graduationYear = item.graduationYear,
            //        currentPosition = item.currentPosition,
            //        isActive = item.isActive,
            //        iterator = index + 1 + students.Count()
            //    });

            //var combined = studentsWithIterator.Concat(alumniWithIterator);

            //return Ok(combined);
            #endregion

            var result = (from l in _context.Logins
                          join s in _context.Students on l.ReferenceId equals s.Id into studentJoin
                          from s in studentJoin.DefaultIfEmpty()
                          join a in _context.Alumni on l.ReferenceId equals a.Id into alumniJoin
                          from a in alumniJoin.DefaultIfEmpty()
                          where s != null || a != null
                          select new
                          {
                              Id = s != null ? s.Id : a.Id,
                              l.Email,
                              Name = s != null ? s.Name : a.Name,
                              ProfileId = s != null ? s.StudentId : a.StudentId,
                              userType = l.UserType,
                              currentPosition = a.CurrentPosition,
                              graduationYear = a.YearGraduated,
                              course = s.course,
                              loginId = l.Id,
                              isActive = s != null ? s.isActive : a.isActive
                          }).ToList()
                          .Select((x,index) => new StudAlum
                          {
                              Id =x.Id,
                              Name =x.Name,
                              userType = x.userType.ToLower(),
                              ProfileId = x.ProfileId.ToString(),
                              Email = x.Email,
                              currentPosition = x.currentPosition,
                              graduationYear = x.graduationYear,
                              course = x.course,
                              loginId = x.loginId,
                              isActive=x.isActive,
                              iterator = index + 1
                          }).ToList();

            return Ok(result.Where(c => c.userType.ToLower() == "student" || c.userType.ToLower() == "alumni"));
        }

        [HttpPost("updateFaculty")]
        public async Task<ActionResult<OperationResult>> updateFaculty(FacultyDto dto, [FromQuery] bool? isSupChanged)
        {

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var faculty = await _context.Faculties.FirstOrDefaultAsync(c => c.Id == dto.Id);

                if (faculty == null)
                    return BadRequest(new OperationResult(false, "Faculty account not found!"));


                var oldEmail = faculty.InstitutionalEmail;

                faculty.InstitutionalEmail = dto.Email;
                faculty.Name = dto.FullName;
                faculty.FacultyId = int.Parse(dto.ProfileId);
                faculty.Position = dto.position;
                faculty.Department = dto.department;

                if (!string.IsNullOrWhiteSpace(dto.Password))
                {
                    var login = await _context.Logins.FirstOrDefaultAsync(a => a.Email == oldEmail);
                    if (dto.Email != oldEmail)
                    {
                        login.Email = dto.Email;
                    }
                    //login.Password = _passwordHasher.HashPassword(dto.Password);
                    login.Password = dto.Password;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new OperationResult(true, "Success!"));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new OperationResult(false, "Error!"));
            }
        }

        [HttpPost("updateOrg")]
        public async Task<ActionResult<OperationResult>> updateOrg(OrgDto dto)
        {

            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var org = await _context.Organizations.FirstOrDefaultAsync(c => c.Id == dto.Id);

                if (org == null)
                    return BadRequest(new OperationResult(false, "Organization account not found!"));


                var oldEmail = org.OrganizationEmail;

                org.OrganizationId = dto.ProfileId;
                org.OrganizationEmail = dto.Email;
                org.OrganizationName = dto.FullName;


                if (!string.IsNullOrWhiteSpace(dto.Password))
                {
                    var login = await _context.Logins.FirstOrDefaultAsync(a => a.Email == oldEmail);
                    if (dto.Email != oldEmail)
                    {
                        login.Email = dto.Email;
                    }
                    //login.Password = _passwordHasher.HashPassword(dto.Password);
                    login.Password = dto.Password;
                }

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return Ok(new OperationResult(true, "Success!"));
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return BadRequest(new OperationResult(false, "Error!"));
            }
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<OperationResult>> DeleteUser(int? id)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                var user = await _context.Logins.FindAsync(id);

                if (user == null)
                    return new OperationResult(false, "Account not found");


                if(user.UserType.ToLower() == "student")
                {
                    var student = await _context.Students.FirstOrDefaultAsync(c => c.Id == user.ReferenceId);
                    _context.Students.Remove(student);
                }
                else if(user.UserType.ToLower() == "alumni")
                {
                    var alumni = await _context.Alumni.FirstOrDefaultAsync(c => c.Id == user.ReferenceId);
                    _context.Alumni.Remove(alumni);
                }

                _context.Logins.Remove(user);

                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
            }



            return new OperationResult(true, "");
        }

    }
}
