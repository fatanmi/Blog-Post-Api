using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Implementation.Contract;
using ServerLibrary.Model.Entities;
using ServerLibrary.Model.DTO;

namespace Blog_Post_Api.Controllers
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
        public async Task<IActionResult> GetAllAsync()
        {
            try
            {

                IEnumerable<Comment> userComment = await _unitOfWork.Comments.GetAllAsync();
                if (userComment != null)
                {
                    return Ok(userComment);
                }
                return Ok(userComment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred retriving post at {nameof(GetAllAsync)} ");
                return BadRequest(ex);
            }
        }

        [HttpGet("{PostId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Get(int PostId)
        {
            try
            {

                IEnumerable<Comment> userComment = await _unitOfWork.Comments.GetAllAsync(q => q.PostId == PostId);
                if (userComment != null)
                {
                    return Ok(userComment);
                }
                return Ok(userComment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred retriving post at {nameof(Get)} ");
                return BadRequest(ex);
            }
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
            try
            {
                Comment comment = _mapper.Map<Comment>(userComment);

                await _unitOfWork.Comments.Insert(comment);
                return Ok(userComment);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred retriving post at {nameof(GetAllAsync)} ");
                return BadRequest(ex);
            }
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
            try
            {
                Comment ExistingPost = await _unitOfWork.Comments.GetAsync(q => q.Id == Id);
                if (ExistingPost != null)
                {
                    _mapper.Map(UpdateComment, ExistingPost);
                    _unitOfWork.Comments.UpdateAsync(ExistingPost);
                    await _unitOfWork.Save();

                    return Ok(UpdateComment);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred retriving post at {nameof(UpdateComment)} ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok();
        }
        [HttpDelete("{Id:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteComment(int Id)
        {
            try
            {
                Comment ExistingPost = await _unitOfWork.Comments.GetAsync(q => q.Id == Id);
                if (ExistingPost != null)
                {
                    await _unitOfWork.Comments.DeleteAsync(Id);
                    await _unitOfWork.Save();

                    return StatusCode(200, "Comment Deleted!");
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error occurred retriving post at {nameof(DeleteComment)} ");
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
            return Ok();
        }

    }
}
