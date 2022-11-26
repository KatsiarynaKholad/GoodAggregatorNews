using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.WebAPI.Models.Requests;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace GoodAggregatorNews.WebAPI.Controllers
{
    /// <summary>
    /// Controller for work with article
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ArticlesController : ControllerBase
    {
        private readonly IArticleService _articleService;
        private readonly ISourceService _sourceService;


        public ArticlesController(IArticleService articleService, 
            ISourceService sourceService)
        {
            _articleService = articleService;
            _sourceService = sourceService;
        }

        /// <summary>
        /// Get article by specified id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> GetArticleById(Guid id)
        {
            try
            {
                var article = await _articleService.GetArticleByIdAsync(id);
                if (article!=null)
                {
                    return Ok(article);
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetArticleById in ArticleController is not successful");
                throw;
            }
        }

        /// <summary>
        /// Get articles by name and source id
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpGet]
        [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetAllArticles([FromQuery]GetArticlesRequestModel? model)
        {
            try
            {
                 var articles = await _articleService
                    .GetArticlesByNameAndSourcesAsync(model?.Name, model?.SourceId);

                if (articles!=null)
                {
                    return Ok(articles);
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetAllArticles in ArticleController is not successful");
                throw;
            }
        }

        /// <summary>
        /// Add Articles (only Role: Admin)
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        //[Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(ArticleDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> AddArticles()
        {
            try
            {
                var sources = await _sourceService.GetSourceAsync();

                if (sources.Any())
                {
                    foreach (var source in sources)
                    {
                        await _articleService.GetAllArticleDataFromRssAsync();
                        await _articleService.AddArticleTextToArticleAsync();
                        //RecurringJob.AddOrUpdate(()=>_articleService.GetAllArticleDataFromRssAsync(),
                        //    "*/20 * * * *");
                        //RecurringJob.AddOrUpdate(() => _articleService.AddArticleTextToArticleAsync(),
                        //    "*/30 * * * *");
                    }

                    return Ok();
                }
                return BadRequest();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: AddArticles in ArticleController is not successful");
                throw;
            }
        }


        /// <summary>
        /// Delete article (only Role: Admin)
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorize(Roles = "Admin")]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> DeleteArticle(Guid id)
        {
            try
            {
                if (!Guid.Empty.Equals(id))
                {
                    await _articleService.DeleteArticleAsync(id);
                    return Ok();
                }
                return NotFound();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetArticleById in ArticleController is not successful");
                throw;
            }
        }

    }
}
