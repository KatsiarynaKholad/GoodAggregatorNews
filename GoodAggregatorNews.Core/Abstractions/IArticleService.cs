using GoodAggregatorNews.Core.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Core.Abstractions
{
    public interface IArticleService
    {
        Task<List<ArticleDto>> GetArticlesByPageNumberAndPageSizeAsync();    
        Task<ArticleDto> GetArticleByIdAsync(Guid id);
        Task<int> CreateArticleAsync(ArticleDto dto);
        Task<int> PatchAsync(Guid modelId, List<PatchModel> patchList);
        Task<List<ArticleDto>> GetArticlesByNameAndSourcesAsync(string? name, Guid? sourceId);
        Task DeleteArticleAsync(Guid id);
        Task GetAllArticleDataFromRssAsync();
        Task AddArticleTextToArticleAsync();
        Task AggregateArticlesFromExternalSourceAsync();
        Task AddRateToArticlesAsync();
    }
}
