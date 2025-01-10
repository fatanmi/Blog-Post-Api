using AutoMapper;
using Blog_Post_Api.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Implementation.Contract;
using ServerLibrary.Model.DTO;

namespace Blog_Post_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApiUser> _UserManager;
        private readonly ILogger<AccountController> _Logger;
        private readonly IMapper _Mapper;
        private readonly IAuthManager _AuthManager;

        public AccountController(
            UserManager<ApiUser> UserManager, ILogger<AccountController> Logger, IMapper Mapper, IAuthManager AuthManager)
        {
            _UserManager = UserManager;
            _Logger = Logger;
            _Mapper = Mapper;
            _AuthManager = AuthManager;
        }

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetUsers()
        {

            try
            {
                List<ApiUser> UsersList = await _UserManager.Users.ToListAsync();
                var userDtos = _Mapper.Map<List<UserDTO>>(UsersList);

                return Ok(userDtos); ;

            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error occurred at {nameof(GetUsers)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");
            }
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Register([FromBody] CreateUserDTO User)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var UserDetails = _Mapper.Map<ApiUser>(User);
                UserDetails.UserName = User.Email;
                IdentityResult Result = await _UserManager.CreateAsync(UserDetails, User.Password);
                if (!Result.Succeeded)
                {

                    return BadRequest(new { Errors = Result.Errors.Select(e => e.Description) });
                }
                await _UserManager.AddToRolesAsync(UserDetails, User.Roles);
                return Accepted();

            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error occurred at {nameof(Register)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");

            }
        }
        [HttpPost]
        [Route("login")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LoginUserDTO User)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                if (!await _AuthManager.ValidateUser(User))
                {
                    return Unauthorized();
                }



                return Accepted(new { Token = await _AuthManager.CreateToken() });

            }
            catch (Exception ex)
            {
                _Logger.LogError(ex, $"Error occurred at {nameof(Register)}");
                return StatusCode(StatusCodes.Status500InternalServerError, "An error occurred while processing your request.");

            }
        }
    }
}
