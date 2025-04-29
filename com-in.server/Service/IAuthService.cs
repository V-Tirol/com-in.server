using com_in.server.DTO;
using com_in.server.Hasher;
using com_in.server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;

namespace com_in.server.Service
{
    public interface IAuthService
    {
        Task<OperationResult> RegisterAsync(RegistrationDto registrationDto);
        Task<UserInfoDto> AuthenticationAsync(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly ForumContext _context;
        private readonly IPasswordHasher _passwordHasher;

        public AuthService(ForumContext context, IPasswordHasher passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<OperationResult> RegisterAsync(RegistrationDto registrationDto)
        {
            // Check if email already exists in the Login table
            var existingUser = await _context.Logins
                .FirstOrDefaultAsync(l => l.Email == registrationDto.Email);

            if (existingUser != null)
            {
                return new OperationResult(false, "Email already in use.");
            }

            // Step 1: Create the Login record
            var login = new Login
            {
                Email = registrationDto.Email,
                Password = _passwordHasher.HashPassword(registrationDto.Password), // Hash password
                UserType = registrationDto.UserType,
            };

            _context.Logins.Add(login);
            await _context.SaveChangesAsync();

            // Step 2: Create the profile record based on the UserType (Student, Faculty, etc.)
            if (registrationDto.UserType == "Student")
            {
                var student = new Student
                {
                    StudentId = registrationDto.ProfileId,
                    Name = registrationDto.FullName,
                    Email = registrationDto.Email,
                    Course = registrationDto.Course, // Specific to Student (e.g., course name)
                    isActive = true
                };

                _context.Students.Add(student);
                await _context.SaveChangesAsync();

                // Associate login record with the student profile
                login.ReferenceId = student.Id; // Save the profile reference in Login
            }
            else if (registrationDto.UserType == "Faculty")
            {
                var faculty = new Faculty
                {
                    FacultyId = registrationDto.ProfileId,
                    Position = registrationDto.Position,
                    Name = registrationDto.FullName,
                    InstitutionalEmail = registrationDto.Email,
                    Department = registrationDto.Department, // Specific to Faculty (e.g., department)
                    isActive = true
                };

                _context.Faculties.Add(faculty);
                await _context.SaveChangesAsync();

                login.ReferenceId = faculty.Id; // Save the profile reference in Login
            }
            else if (registrationDto.UserType == "Alumni")
            {
                var alumni = new Alumni
                {
                    Name = registrationDto.FullName,
                    Email = registrationDto.Email,
                    CurrentPosition = registrationDto.Position,
                    StudentId = registrationDto.ProfileId,
                    YearGraduated = registrationDto.YearGraduated,
                    isActive = true
                };
                
                _context.Alumni.Add(alumni);
                await _context.SaveChangesAsync();

                login.ReferenceId = alumni.Id; // Save the profile reference in Login
            }
            else if (registrationDto.UserType == "Organization")
            {
                var org = new Organization
                {
                    OrganizationEmail = registrationDto.Email,
                    OrganizationId = registrationDto.ProfileId,
                    OrganizationName = registrationDto.FullName,
                    isActive = true
                };

                _context.Organizations.Add(org);
                await _context.SaveChangesAsync();

                login.ReferenceId = org.Id; // Save the profile reference in Login
            }
            else if (registrationDto.UserType == "Admin")
            {
                var admin = new Admin
                {
                    InstitutionalEmail = registrationDto.Email,
                    Name = registrationDto.FullName,
                    Position = registrationDto.Position,
                    isActive = true
                };

                _context.Admins.Add(admin);
                await _context.SaveChangesAsync();

                login.ReferenceId = admin.Id; // Save the profile reference in Login
            }

                await _context.SaveChangesAsync();

            return new OperationResult(true, "Registration successful.");
        }

        public async Task<UserInfoDto> AuthenticationAsync(string email, string password)
        {
            var user = await _context.Logins.FirstOrDefaultAsync(c => c.Email == email);
            
            if(user == null)
            {
                return null;
            }

            if (!_passwordHasher.VerifyPassword(user.Password, password))
            {
                return null;
            }

            object profile;

            if(user.UserType == "Student")
            {
                profile = await _context.Students.FirstOrDefaultAsync(c => c.Id == user.Id);
                return new UserInfoDto
                {
                    student = (Student)profile
                };
            }
            else if(user.UserType == "Admin")
            {
                profile = await _context.Admins.FirstOrDefaultAsync(c => c.Id == user.Id);
                return new UserInfoDto
                {
                    admin = (Admin)profile
                };
            }
            else if (user.UserType == "Faculty")
            {
                profile = await _context.Faculties.FirstOrDefaultAsync(c => c.Id == user.Id);
                return new UserInfoDto
                {
                    faculty = (Faculty)profile
                };
            }
            else if (user.UserType == "Alumni")
            {
                profile = await _context.Alumni.FirstOrDefaultAsync(c => c.Id == user.Id);
                return new UserInfoDto
                {
                    alumni = (Alumni)profile
                };
            }
            else if (user.UserType == "Organization")
            {
                profile = await _context.Organizations.FirstOrDefaultAsync(c => c.Id == user.Id);
                return new UserInfoDto
                {
                    organization = (Organization)profile
                };
            }

            return null;
        }
    }

    public class OperationResult
    {
        public bool IsSuccess { get; set; }
        public string Message { get; set; }

        public OperationResult(bool isSuccess, string message)
        {
            IsSuccess = isSuccess;
            Message = message;
        }
    }
}
