using com_in.server.DTO;
using com_in.server.Models;
using com_in.server.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;

namespace com_in.server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public async Task<ActionResult> LoginAsync(string email, string password)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest("Invalid Data"); //return error for validation
            }

            var result = await _authService.AuthenticationAsync(email, password);

            if(result == null)
            {
                return BadRequest("Account not Found!");
            }
            return Ok(result);
        }

        [HttpPost("register")]
        public async Task<ActionResult<Login>> Register([FromBody] RegistrationDto registrationDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return validation error
            }

            var result = await _authService.RegisterAsync(registrationDto);

            if (!result.IsSuccess)
            {
                return BadRequest(result.Message); // message for login fail, (e.g. email already exists)
            }
            return Ok(result.Message);
        }

        [HttpGet("confirm-email")]
        public async Task<ActionResult> ConfirmEmail([FromQuery] int userId, [FromQuery] string token)
        {
            if(string.IsNullOrEmpty(token))
            {
                return BadRequest("Invalid confirmation link");
            }

            var result = await _authService.ConfirmEmailAsync(userId, token);

            if(!result.IsSuccess)
            {
                return BadRequest(result.Message);
            }

            return Ok(result.Message);
        }
    }
}
