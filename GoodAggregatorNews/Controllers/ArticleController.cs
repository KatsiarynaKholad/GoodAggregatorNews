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
        //private readonly ISourceService _sourceService;
        //private readonly ICommentService _commentService;
        private readonly IMapper _mapper;
        public ArticleController(IArticleService articleService,
            //ISourceService sourceService,
            //ICommentService commentService,
            IMapper mapper)
        {
            _articleService = articleService;
            //_sourceService = sourceService;
            //_commentService = commentService;
            _mapper = mapper;
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
                    var sortedArticles = articles.OrderByDescending(a => a.PublicationDate)
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
