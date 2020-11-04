using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Api.Infrastructure.Helpers;
using Api.Infrastructure.RequestModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Services.Services;

namespace Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IConfiguration _configuration;
        private string tmpToken;

        public AuthController(IAuthService authService, IConfiguration configuration)
        {
            _authService = authService;
            _configuration = configuration;
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestModel userForLoginDto)
        {
            var user = await _authService.Login(userForLoginDto.Email, userForLoginDto.Password);

            if (user == null)
            {
                ModelState.AddModelError("LoginFailed", "The password or email that you've entered is incorrect.");
                return BadRequest(ModelState);
            }

            var tokenString = TokenManager.GenerateToken(userForLoginDto, user.Id, _configuration);
            tmpToken = tokenString;
            return Ok(new { tokenString, user });
        }

        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel registerRequestModel)
        {
            if (await _authService.GetUserByEmail(registerRequestModel.Email) != null)
            {
                ModelState.AddModelError("Email", "Email already exists");
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userToCreate = new Core.Models.User
            {
                Firstname = registerRequestModel.Firstname,
                Lastname = registerRequestModel.Lastname,
                Email = registerRequestModel.Email
            };

            var createdUser = await _authService.Register(userToCreate, registerRequestModel.Password);

            return Ok(createdUser);
        }

        [HttpGet("Find")]
        public async Task<IActionResult> Find([FromQuery] string searchText)
        {
            var user = new Core.Models.User();
            if (!String.IsNullOrWhiteSpace(searchText))
            {
                user = await _authService.GetUserByEmail(searchText);
                if (user == null)
                {
                    user = await _authService.GetUserByUsername(searchText);
                }

                if (user == null)
                {
                    var tempGuid = new Guid();

                    if (Guid.TryParse(searchText.ToCharArray(), out tempGuid))
                    {
                        user = await _authService.GetUserById(searchText);
                    }
                }
            }
            if (user == null)
            {
                ModelState.AddModelError("NotFound", "User not found");
                return BadRequest(ModelState);
            }

            return Ok(user);
        }

        [HttpGet("GetCurrentUser")]
        public async Task<IActionResult> GetCurrentUser(string token)
        {
            var data = TokenManager.GetPrincipal(token, _configuration);

            var result = await _authService.GetUserById(data.Claims.FirstOrDefault(i => i.Type == "UserId").Value);

            return Ok(result);
        }

        [HttpGet("Exists")]
        public async Task<IActionResult> Exists(string id)
        {
            var result = await _authService.Exists(id);

            return Ok(result);
        }
    }
}
