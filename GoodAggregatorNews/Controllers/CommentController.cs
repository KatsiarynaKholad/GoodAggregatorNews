using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GoodAggregatorNews.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IClientService _clientService;
        private readonly IRoleService _roleService;

        public CommentController(ICommentService commentService,
                IClientService clientService,
                IRoleService roleService)
        {
            _commentService = commentService;
            _clientService = clientService;
            _roleService = roleService;
        }

        [HttpGet]
        public async Task<IActionResult> ListComments(Guid id)   //??
        {
            try
            {
                var comments = await _commentService.FindCommentsByArticleIdAsync(id);
                if (comments!=null)
                {
                    return View(new CommentsListWithArticle()
                    {
                        Comments = comments,
                        ArticleId = id
                    });
                }
                return NotFound("Comments not found");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: ListComments is not successful");
                throw;
            }
        }

        //[HttpPost]
        //public async Task<IActionResult> Create(CreateComment comment)
        //{
        //    try
        //    {
        //        var clientIsExist = await _clientService.IsUserExists(comment.ClientId);
        //        if (clientIsExist)
        //        {
        //            var client = await _clientService.

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, "Operation: Create is not successful");
        //        throw;
        //    }
        //}
        //public IActionResult Index()
        //{
        //    return View();
        //}
    }
}
