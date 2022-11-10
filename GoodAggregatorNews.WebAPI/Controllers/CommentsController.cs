using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GoodAggregatorNews.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with comments
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentService _commentService;
        public CommentsController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Get comment by article id
        /// </summary>
        /// <param name="articleId"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetCommentByArticle(Guid articleId)
        {
            try
            {
                if (!Guid.Empty.Equals(articleId))
                {
                    var comments = await _commentService.FindCommentsByArticleIdAsync(articleId);
                    if (comments != null)
                    {
                        return Ok(comments);
                    }
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetCommentByArticle in CommentController is not successful");
                throw;
            }
        }


        /// <summary>
        /// Add comment by article (only authorize)
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddCommentByArticle(CommentDto dto)
        {
            try
            {
                if (dto!=null)
                {
                    var comment = await _commentService.AddCommentAsync(dto);
                    if (comment>0)
                    {
                        return Ok();
                    }
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: AddCommentByArticle in CommentsController is not successful");
                throw;
            }
        }

        /// <summary>
        /// Delete comment (only Role: Admit)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteComment(Guid id)
        {
            try
            {
                if (!Guid.Empty.Equals(id))
                {
                    await _commentService.DeleteCommentAsync(id);
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: DeleteComment in CommentsController is not successful");
                throw;
            }
        }
    }
}
