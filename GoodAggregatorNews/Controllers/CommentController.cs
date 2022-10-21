using AutoMapper;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Security.Claims;

namespace GoodAggregatorNews.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly IClientService _clientService;
        private readonly IRoleService _roleService;
        private readonly IArticleService _articleService;

        public CommentController(ICommentService commentService,
                IClientService clientService,
                IRoleService roleService,
                IArticleService articleService)
        {
            _commentService = commentService;
            _clientService = clientService;
            _roleService = roleService;
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> ListComments(Guid id)
        {
            try
            {
                var comments = await _commentService.FindCommentsByArticleIdAsync(id);
                if (comments != null)
                {
                    var commentsList = new CommentsListWithArticle
                    {
                        Comments = comments,
                        ArticleId = id
                    };

                    return View(commentsList);
                }
                return NotFound("Comments not found");
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: ListComments is not successful");
                throw;
            }
        }


        [HttpGet]
        public IActionResult CreateComment(Guid id)       //??
        {
            return View();
        }

        //[HttpGet]
        //public async Task<IActionResult> CreateComment(Guid id)       
        //{
        //    try
        //    {
        //        if (id != Guid.Empty)
        //        {
        //            var comments = await _commentService.FindCommentsByArticleIdAsync(id);
        //            if (HttpContext.User.Identity.Name != null)
        //            {
        //                var clientEmail = HttpContext.User.Identity.Name;
        //                var client = await _clientService.GetUserByEmailAsync(clientEmail);
        //                var roleName = await _roleService.GetRoleNameById(client.RoleId);

        //                return View(new CommentsListWithArticle
        //                {
        //                    ArticleId = id,
        //                    Comments = comments
        //                });
        //            }
        //            else
        //            {
        //                return Ok();
        //            }
        //        }
        //        return Ok();
        //    }
        //    catch (Exception ex)
        //    {
        //        Log.Error(ex, "Operation: CreateComment is not successful");
        //        return Ok();
        //    }

        //}




        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateComment(CreateComment comment)    //??
        {
            try
            {
                var clientEmail = HttpContext.User.Identity.Name;

                if (clientEmail != null)
                {
                    var client = await _clientService.GetUserByEmailAsync(clientEmail);
                    var roleName = await _roleService.GetRoleNameById(client.RoleId);

                    CommentDto commentDto = new CommentDto()
                    {
                        FullName = client.Name,
                        Id = Guid.NewGuid(),
                        ArticleId = comment.ArticleId,
                        PublicationDate = DateTime.Now,
                        Text = comment.Text,
                        ClientId = client.Id
                    };

                    await _commentService.AddCommentAsync(commentDto);

                    return Ok();

                }

                else
                {
                    return BadRequest();
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: Create is not successful");
                throw;
            }
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}
