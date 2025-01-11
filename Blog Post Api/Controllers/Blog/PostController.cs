using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Implementation.Contract;
using ServerLibrary.Model.Entities;
using ServerLibrary.Model.DTO;
using Microsoft.AspNetCore.Authorization;
using ServerLibrary.Implementation.Repositories;

namespace Blog_Post_Api.Controllers.Blog
{
    /// <summary>
    /// Endpoint for starting a post.
    /// </summary>
    /// /// <remarks>
    /// This endpoint creates a new post in the system.
    /// </remarks>
    /// <response code="201">Post created successfully.</response>
    /// <response code="400">Validation failed.</response>
    [Route("api/[controller]")]
    //[Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ApiController]
    public class PostController : ControllerBase
    {
        //private readonly AppContext _dbContext;
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<PostController> _logger;

        private readonly IMapper _mapper;
        public PostController(ILogger<PostController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }



        [HttpGet]

        public async Task<IActionResult> GetAllAsync([FromQuery] RequestParams requestParams)
        {

            IEnumerable<Post> userPost = await _unitOfWork.Posts.GetAllAsync(requestParams: requestParams, includes: new List<string> { "Comments" });
            if (userPost != null)
            {
                return Ok(userPost);
            }
            return Ok(userPost);

        }


        [HttpGet("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int Id)
        {
            try
            {

                var userPost = await _unitOfWork.Posts.GetAsync(q => q.Id == Id, includes: new List<string> { "Comments" });
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
        [HttpPost]
        public async Task<IActionResult> CreatePost([FromBody] CreatePostDTO userPost)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Post post = _mapper.Map<Post>(userPost);

            await _unitOfWork.Posts.Insert(post);
            return Ok(userPost);

        }
        [HttpPut("{Id:int}")]
        public async Task<IActionResult> UpdatePost([FromBody] UpdatePostDTO UpdatePost, int Id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Post ExistingPost = await _unitOfWork.Posts.GetAsync(q => q.Id == Id);
            if (ExistingPost != null)
            {
                _mapper.Map(UpdatePost, ExistingPost);
                _unitOfWork.Posts.UpdateAsync(ExistingPost);
                await _unitOfWork.Save();

                return Ok(UpdatePost);
            }


            return Ok();
        }
        [HttpDelete("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeletePost(int Id)
        {
            Post ExistingPost = await _unitOfWork.Posts.GetAsync(q => q.Id == Id);
            if (ExistingPost != null)
            {
                await _unitOfWork.Posts.DeleteAsync(Id);
                await _unitOfWork.Save();

                return StatusCode(200, "Post Deleted!");
            }
            return Ok();
        }
    }
}
