using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Implementation.Contract;
using ServerLibrary.Model.Entities;
using ServerLibrary.Model.DTO;
using ServerLibrary.Implementation.Repositories;

namespace Blog_Post_Api.Controllers.Blog
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CommentController> _logger;

        private readonly IMapper _mapper;
        public CommentController(ILogger<CommentController> logger, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAllAsync([FromQuery] RequestParams requestParams)
        {
            IEnumerable<Comment> userComment = await _unitOfWork.Comments.GetAllAsync(requestParams: requestParams);
            if (userComment != null)
            {
                return Ok(userComment);
            }
            return Ok(userComment);

        }

        [HttpGet("{PostId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int PostId)
        {
            IEnumerable<Comment> userComment = await _unitOfWork.Comments.GetAllAsync(expression: q => q.PostId == PostId);
            if (userComment != null)
            {
                return Ok(userComment);
            }
            return Ok(userComment);
        }
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateComment([FromBody] CreateCommentDTO userComment)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Comment comment = _mapper.Map<Comment>(userComment);

            await _unitOfWork.Comments.Insert(comment);
            return Ok(userComment);


        }
        [HttpPut("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdatePost([FromBody] UpdateCommentDTO UpdateComment, int Id)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Comment ExistingPost = await _unitOfWork.Comments.GetAsync(q => q.Id == Id);
            if (ExistingPost != null)
            {
                _mapper.Map(UpdateComment, ExistingPost);
                _unitOfWork.Comments.UpdateAsync(ExistingPost);
                await _unitOfWork.Save();

                return Ok(UpdateComment);
            }

            return Ok();
        }
        [HttpDelete("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment(int Id)
        {
            Comment ExistingPost = await _unitOfWork.Comments.GetAsync(q => q.Id == Id);
            if (ExistingPost != null)
            {
                await _unitOfWork.Comments.DeleteAsync(Id);
                await _unitOfWork.Save();

                return StatusCode(200, "Comment Deleted!");
            }

            return Ok();
        }

    }
}
