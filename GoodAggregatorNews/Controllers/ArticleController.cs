using AutoMapper;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GoodAggregatorNews.Controllers
{
    public class ArticleController : Controller
    {
        private readonly IArticleService _articleService;
        public ArticleController(IArticleService articleService)
        {
            _articleService = articleService;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var articles = await _articleService
                    .GetArticlesByPageNumberAndPageSizeAsync();       

                if (articles.Any())
                {
                    var sortedArticles = articles.Where(art=>art.FullText!=null)
                        .OrderByDescending(art=>art.PublicationDate)
                        .ThenByDescending(a => a.Rate)
                        .ToList();

                    return View(sortedArticles);
                }
                else
                {
                    return View("NoArticles");
                }

            }
            catch (Exception ex)
            {
                return StatusCode(404);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            try
            {
                var articleDto = await _articleService.GetArticleByIdAsync(id);
                if (articleDto != null)
                {
                    return View(articleDto);
                }
                else
                {
                    return NotFound();
                }
            }
            catch (Exception ex)
            {
                return StatusCode(404);
            }
        }

        

    }
}
