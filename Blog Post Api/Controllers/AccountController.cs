using AutoMapper;
using Blog_Post_Api.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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

        public AccountController(UserManager<ApiUser> UserManager, ILogger<AccountController> Logger, IMapper Mapper)
        {
            _Logger = Logger;
            _Mapper = Mapper;
            _UserManager = UserManager;
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
    }
}
