using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using ServerLibrary.Implementation.Contract;
using ServerLibrary.Model.Entities;
using System.Threading.Tasks;
using System;
using ServerLibrary.Model.DTO;

namespace Blog_Post_Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostController : ControllerBase
    {
        //private readonly AppContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PostController> _logger;

        private IMapper _mapper;
        public PostController(ILogger<PostController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet("{id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {

                Post userPost = await _unitOfWork.Posts.GetAsync(q => q.Id == Id);
                if (userPost != null)
                {
                    return Ok(userPost);
                }
                return Ok(userPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred retriving post at {nameof(Get)} ");
                return BadRequest(ex);
            }
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync(int Id)
        {
            try
            {

                IEnumerable<Post> userPost = await _unitOfWork.Posts.GetAllAsync();
                if (userPost != null)
                {
                    return Ok(userPost);
                }
                return Ok(userPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred retriving post at {nameof(GetAllAsync)} ");
                return BadRequest(ex);
            }
        }
        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreatePost([FromBody] CreatePost userPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                Post post = _mapper.Map<Post>(userPost);

                await _unitOfWork.Posts.Insert(post);
                return Ok(userPost);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred retriving post at {nameof(GetAllAsync)} ");
                return BadRequest(ex);
            }
        }
    }
}
