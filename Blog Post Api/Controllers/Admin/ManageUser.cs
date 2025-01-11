using AutoMapper;
using Blog_Post_Api.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ServerLibrary.Implementation.Contract;
using ServerLibrary.Model.DTO;

namespace Blog_Post_Api.Controllers.Admin
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ManageUser : ControllerBase
    {
        private readonly UserManager<ApiUser> _UserManager;
        private readonly ILogger<ManageUser> _Logger;
        private readonly IMapper _Mapper;
        private readonly IAuthManager _AuthManager;

        public ManageUser(
            UserManager<ApiUser> UserManager, ILogger<ManageUser> Logger, IMapper Mapper, IAuthManager AuthManager)
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

    }
}
