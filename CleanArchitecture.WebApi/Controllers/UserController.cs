using CleanArchitecture.Application.Dto;
using CleanArchitecture.Infrastructure.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IJwtProvider _jwtProvider;

        public UserController(UserManager<IdentityUser> userManager, IJwtProvider jwtProvider)
        {
            _userManager = userManager;
            _jwtProvider = jwtProvider;
        }

        /// <summary>
        /// For User Registration
        /// </summary>
        /// <response code="201">User is Register Successfully</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [HttpPost]
        [Route("Register")]
        // public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto user)
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequestDto user)
        {

            var userExist = await _userManager.FindByEmailAsync(user.Email);

            if (userExist != null)
            {
                return BadRequest(new AuthResult
                {
                    Token = "",
                    RefreshToken = "",
                    Error = "User Already Exist"
                });
            }

            var newUser = new IdentityUser
            {
                UserName = user.Name,
                Email = user.Email
            };

            var result = await _userManager.CreateAsync(newUser, user.Password);

            if (result.Succeeded)
            {
                return Created("", "UserCreated Successfully");
            }

            return BadRequest("server Error");
        }

        /// <summary>
        /// Login a User
        /// </summary>
        /// <response code="200">User is login Successfully</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        /// <response code="403">Client lack sufficient permission to perform the request</response>
        /// <response code="500">Unexpected Error encountered</response>
        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login([FromBody] UserLoginRequestDto loginRequest)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(loginRequest.Email);

                if (user == null)
                {
                    return BadRequest(new AuthResult
                    {
                        Token = "",
                        RefreshToken = "",
                        Error = "Invalid UserName"
                    });
                }

                var userExist = await _userManager.CheckPasswordAsync(user, loginRequest.Password);

                if (!userExist)
                {
                    return BadRequest(new AuthResult
                    {
                        Token = "",
                        RefreshToken = "",
                        Error = "Invalid Credentials"
                    });
                }

                var token = await _jwtProvider.Generate(user);

                return Ok(token);
            }

            return BadRequest("Invalid Payload");
        }

        /// <summary>
        /// For Token Refreshment
        /// </summary>
        /// <response code="200">Token Refresh successfully</response>
        /// <response code="400">The server was unable to process the request</response>
        /// <response code="401">Client is not authenticated</response>
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenRequest tokenResult)
        {
            var token = await _jwtProvider.VerifyGenerateToken(tokenResult);

            return Ok(token);

        }
    }
}