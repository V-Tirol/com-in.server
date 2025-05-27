using com_in.server.DTO;
using com_in.server.Hasher;
using com_in.server.Helper;
using com_in.server.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Transactions;

namespace com_in.server.Service
{
    public interface IAuthService
    {
        Task<OperationResult> RegisterAsync(RegistrationDto registrationDto);
        Task<OperationResult> ConfirmEmailAsync(int userId, string token);
        Task<UserInfoDto> AuthenticationAsync(string email, string password);
    }

    public class AuthService : IAuthService
    {
        private readonly ForumContext _context;
        private readonly IPasswordHasher _passwordHasher;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;

        public AuthService(ForumContext context, IPasswordHasher passwordHasher, IEmailService emailService, IConfiguration configuration)
        {
            _context = context;
            _passwordHasher = passwordHasher;
            _emailService = emailService;
            _configuration = configuration;
        }

        public async Task<OperationResult> RegisterAsync(RegistrationDto registrationDto)
        {
            await using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Check if email already exists in the Login table
                var existingUser = await _context.Logins
                    .FirstOrDefaultAsync(l => l.Email == registrationDto.Email);

                if (existingUser != null)
                {
                    return new OperationResult(false, "Email already in use.");
                }

                // Step 1: Create the Login record

                if (registrationDto.Password != null)
                {
                }


                var login = new Login
                {
                    Email = registrationDto.Email,
                    Password = registrationDto.Password,  //_passwordHasher.HashPassword(registrationDto.Password), // Hash password
                    UserType = registrationDto.UserType,
                    isActive = true

                };

                if (registrationDto.isAddedBySupOrAdmin)
                    login.isEmailConfirmed = true;
                else
                {
                    login.isEmailConfirmed = false;
                    login.EmailConfirmationToken = TokenGenerator.GenerateEmailConfimationToken();
                    login.EmailConfirmationTokenExpiry = DateTime.UtcNow.AddDays(1);
                }

                _context.Logins.Add(login);
                await _context.SaveChangesAsync();

                // Step 2: Create the profile record based on the UserType (Student, Faculty, etc.)
                if (registrationDto.UserType == "Student")
                {
                    var student = new Student
                    {
                        StudentId = int.Parse(registrationDto.ProfileId),
                        Name = registrationDto.FullName,
                        Email = registrationDto.Email,
                        course = registrationDto.course, // Specific to Student (e.g., course name)
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
                        FacultyId = int.Parse(registrationDto.ProfileId),
                        Position = registrationDto.Position,
                        Name = registrationDto.FullName,
                        InstitutionalEmail = registrationDto.Email,
                        Department = registrationDto.department, // Specific to Faculty (e.g., department)
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
                        StudentId = int.Parse(registrationDto.ProfileId),
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
                else if (registrationDto.UserType == "SuperAdmin")
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
                await transaction.CommitAsync();

                if (!registrationDto.isAddedBySupOrAdmin)
                {
                    var confirmationLink = $"{_configuration["AppBaseUrl"]}api/Auth/confirm-email?userId={login.Id}&token={Uri.EscapeDataString(login.EmailConfirmationToken)}";

                    var emailSubject = "Confirm your email";
                    var emailBody = $@"
                            <h1> Welcome to ComIn App</h1>
                            <p>Please confirm your email by clicking the link below: </p>
                            <a href='{confirmationLink}'>Confirm Email</a>
                            <p>If you didn't request this, please ignore this email.";
                    await _emailService.SendEmailAsync(login.Email, emailSubject, emailBody);
                }

                return new OperationResult(true, "Registration successful.");
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return new OperationResult(false, "There was an error in registration.");
            }


        }

        public async Task<OperationResult> ConfirmEmailAsync(int userId, string token)
        {
            var user = await _context.Logins.FindAsync(userId);
            if (user == null)
            {
                return new OperationResult(false, "User not found");
            }

            if (user.isEmailConfirmed)
            {
                return new OperationResult(false, "Email already confirmed");
            }
            if (!TokenGenerator.ValidateTokenExpiry(user.EmailConfirmationTokenExpiry))
            {
                return new OperationResult(false, "Token expired");
            }
            if (user.EmailConfirmationToken != token)
            {
                return new OperationResult(false, "Invalid Token.");
            }

            user.isEmailConfirmed = true;
            user.EmailConfirmationToken = null;
            user.EmailConfirmationTokenExpiry = null;

            await _context.SaveChangesAsync();


            return new OperationResult(true, "Email confirmed");
        }

        public async Task<UserInfoDto> AuthenticationAsync(string email, string password)
        {
            var user = await _context.Logins.FirstOrDefaultAsync(c => c.Email == email);

            if (user == null)
            {
                return null;
            }

            //if (!_passwordHasher.VerifyPassword(user.Password, password))
            //{
            //    return null;
            //}

            if (user.Password != password)
            {
                return null;
            }

            if (!user.isEmailConfirmed)
            {
                return null;
            }



            if (user.UserType == "Student")
            {
                Student profile = await _context.Students.FirstOrDefaultAsync(c => c.Id == user.ReferenceId);
                return new UserInfoDto
                {
                    role = "student",
                    Id = user.Id,
                    Name = profile.Name,
                    Email = profile.Email,
                };
            }
            else if (user.UserType == "SuperAdmin")
            {
                Admin profile = await _context.Admins.FirstOrDefaultAsync(c => c.Id == user.ReferenceId);
                return new UserInfoDto
                {
                    role = "superadmin",
                    Id = user.Id,
                    Name = profile.Name,
                    Email = profile.InstitutionalEmail,
                };
            }
            else if (user.UserType == "Faculty")
            {
                Faculty profile = await _context.Faculties.FirstOrDefaultAsync(c => c.Id == user.ReferenceId);
                return new UserInfoDto
                {
                    role = "faculty",
                    Id = user.Id,
                    Name = profile.Name,
                    Email = profile.InstitutionalEmail,
                };
            }
            else if (user.UserType == "Alumni")
            {
                Alumni profile = await _context.Alumni.FirstOrDefaultAsync(c => c.Id == user.ReferenceId);
                return new UserInfoDto
                {
                    role = "alumni",
                    Id = user.Id,
                    Name = profile.Name,
                    Email = profile.Email,
                };
            }
            else if (user.UserType == "Organization")
            {
                Organization profile = await _context.Organizations.FirstOrDefaultAsync(c => c.Id == user.ReferenceId);
                return new UserInfoDto
                {
                    role = "organization",
                    Id = user.Id,
                    Name = profile.OrganizationName,
                    Email = profile.OrganizationEmail,
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
