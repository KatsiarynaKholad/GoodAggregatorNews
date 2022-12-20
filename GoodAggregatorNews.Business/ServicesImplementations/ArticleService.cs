using AutoMapper;
using GoodAggregatorNews.Abstractions;
using GoodAggregatorNews.Business.Models;
using GoodAggregatorNews.Core;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Data.CQS.Commands;
using GoodAggregatorNews.Data.CQS.Queries;
using GoodAggregatorNews.Database.Entities;
using HtmlAgilityPack;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.ServiceModel.Syndication;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Xml;

namespace GoodAggregatorNews.Business.ServicesImplementations
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IParseService _parseService;
        private readonly IConfiguration _configuration;
        private readonly IMediator _mediator;



        public ArticleService(IMapper mapper, 
            IUnitOfWork unitOfWork, 
            IParseService parseService,
            IConfiguration configuration,
            IMediator mediator)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _parseService = parseService;
            _configuration = configuration;
            _mediator = mediator;
        }

        public async Task<int> CreateArticleAsync(ArticleDto dto)         
        {
            try
            {
                if (dto!=null)
                {
                    var result = await _mediator.Send(new AddArticleCommand() { Dto = dto });
                    return result;
                }
                else
                {
                    throw new ArgumentException(nameof(dto));
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: CreateArticle was not successful");
                throw;
            }
        }

        public async Task<ArticleDto> GetArticleByIdAsync(Guid id)   
        {
            try
            {
                var entity = await _mediator.Send(new GetArticleByIdQuery() { Id = id });
                if (entity!=null)
                {
                    var dto = _mapper.Map<ArticleDto>(entity);
                    return dto;
                }
                return null;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetArticleById was not successful");
                throw;
            }
        }

        public async Task<List<ArticleDto>> GetArticlesByPageNumberAndPageSizeAsync()  
        {
            try
            {
                var list = await _unitOfWork.Articles
                    .Get()
                    .Select(article => _mapper.Map<ArticleDto>(article))
                    .ToListAsync();

                return list;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetArticlesByPageNumberAndPageSize was not successful");
                throw;
            }
        }

        public async Task AggregateArticlesFromExternalSourceAsync()
        {
            try
            {
                var sources = await _mediator.Send(new GetAllSourcesQuery());
               
                if (sources.Any())
                {
                    foreach (var source in sources)
                    {
                        await GetAllArticleDataFromRssAsync(source.Id, source?.RssUrl);
                        await AddArticleTextToArticleAsync();
                    }

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: AggregateArticlesFromExternalSource was not successful");
                throw;
            }
        }
        public async Task<List<ArticleDto>> GetArticlesByNameAndSourcesAsync(string? name, Guid? sourceId)
        {
            try
            {
                var entities = _unitOfWork.Articles.Get();

                if (!string.IsNullOrEmpty(name))
                {
                    entities = entities.Where(dto => dto.Title.Contains(name));
                }

                if (sourceId != null && !Guid.Empty.Equals(sourceId))
                {
                    entities = entities.Where(dto => dto.SourceId.Equals(sourceId));
                }

                var articles = (await entities.ToListAsync())
                    .Select(ent=>_mapper.Map<ArticleDto>(ent))
                    .ToList();

                return articles;
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetArticlesByNameAndSourcesAsync was not successful");
                throw;
            }
        
        }

        public async Task<int> PatchAsync(Guid modelId, List<PatchModel> patchList)
        {
            try
            {
                await _unitOfWork.Articles.PatchAsync(modelId, patchList);
                var result = await _unitOfWork.Commit();
                return result;

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: Patch was not successful");
                throw;
            }
        }

        public async Task DeleteArticleAsync(Guid id)     
        {
            try
            {
                if (!Guid.Empty.Equals(id))
                {
                    var ent = await _unitOfWork.Articles.GetByIdAsync(id);
                    if (ent != null)
                    { 
                        _unitOfWork.Articles.Remove(ent);
                        await _unitOfWork.Commit();
                    }
                }
                else
                {
                    throw new ArgumentException("Operation: delete article is not successful");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: DeleteArticleAsync was not successful");
                throw;
            }
        
        }

        public async Task GetAllArticleDataFromRssAsync()  
        {
            try
            {
                var sources = await _unitOfWork.Sources.GetAllAsync();

                foreach (var source in sources)
                {
                    await GetAllArticleDataFromRssAsync(source.Id, source.RssUrl);
                }
                //Parallel.ForEach(sources, (source) => GetAllArticleDataFromRssAsync(source.Id, source.RssUrl)
                //    .Wait());

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetAllArticleDataFromRss was not successful");
                throw;
            }
        }

        public async Task AddArticleTextToArticleAsync()
        {
            try
            {
                var articlesWithEmptyTextId = await _mediator
                    .Send(new GetAllArticlesWithoutTextIdsQuery());

                if (articlesWithEmptyTextId != null)
                {
                    foreach (var articleId in articlesWithEmptyTextId)
                    {
                        await _parseService.AddArticleTextToArticleAsync(articleId);
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: AddArticleTextToArticleAsync was not successful");
                throw;
            }
        }
        public async Task AddRateToArticlesAsync()     
        {
            try
            {
                var articlesWithEmptyRateId = await _mediator
                    .Send(new GetArticlesWithEmptyRateIdQuery());

                foreach (var articleId in articlesWithEmptyRateId)
                {
                     await RateArticlesAsync(articleId);
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: AddRateToArticlesAsync was not successful");
                throw;
            }
        }

        private async Task RateArticlesAsync(Guid articleId)        
        {
            try
            {
                if (!Guid.Empty.Equals(articleId))
                {
                    var article = await _unitOfWork.Articles.GetByIdAsync(articleId);

                    if (article!=null)
                    {
                        var textWithoutHtml = await RemoveHtmlTags(article.FullText);

                        using (var client = new HttpClient())
                        {
                            var affin = _configuration["Affin"];

                            var httpRequest = new HttpRequestMessage(HttpMethod.Post,
                                new Uri(@"http://api.ispras.ru/texterra/v1/nlp?targetType=lemma&apikey=6c70e0796fde0b04d1a524b084f47b412229b7d8"));

                            httpRequest.Headers.Add("Accept", "application/json");

                            httpRequest.Content = JsonContent.Create(new TextRequestModel[]
                                { new TextRequestModel() {Text = textWithoutHtml}});

                            var response = await client.SendAsync(httpRequest);

                            var responseStr = await response.Content.ReadAsStreamAsync();

                            using (var sr = new StreamReader(responseStr))
                            {
                                var data = await sr.ReadToEndAsync();

                                var resp = JsonConvert
                                    .DeserializeObject<IsprassResponseObject[]>(data);

                                if (resp!=null)
                                {
                                    var affinText = await File.ReadAllTextAsync(@affin);

                                    var dict = JsonConvert.DeserializeObject<Dictionary<string, int?>>(affinText);

                                    if (dict.Any())
                                    {
                                        double countWords = 0;
                                        double intermediateResult = 0;

                                        foreach (var lem in resp[0].Annotations.Lemma)
                                        {
                                            int? rate = 0;
                                            dict.TryGetValue(lem.Value, out rate);

                                            if (rate!=null)
                                            {
                                                intermediateResult += (double)rate;
                                                countWords++;
                                            }
                                        }

                                        if (countWords>0)
                                        {
                                            double result = intermediateResult / countWords;

                                            await _mediator
                                                .Send(new UpdateArticleRateCommand() { ArticleId = articleId, Rate = result });
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    throw new ArgumentException("Article doesn't exist");
                }

            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: RateArticles was not successful");
                throw;
            }
        
        }
        private async Task GetAllArticleDataFromRssAsync(Guid sourceId, string? sourceRssUrl)
        {
            try
            {
                if (!Guid.Empty.Equals(sourceId) && !string.IsNullOrEmpty(sourceRssUrl))
                {
                    var list = new List<ArticleDto>();

                    using (var reader = XmlReader.Create(sourceRssUrl))
                    {
                        var feed = SyndicationFeed.Load(reader);

                        foreach (var item in feed.Items)
                        {
                            var articleDto = new ArticleDto()
                            {
                                Id = Guid.NewGuid(),
                                Title = item.Title.Text,
                                PublicationDate = item.PublishDate.UtcDateTime,
                                ShortDescription = (item.Summary?.Text),
                                Category = item.Categories.FirstOrDefault()?.Name,
                                SourceId = sourceId,
                                SourceUrl = item.Id
                            };

                            list.Add(articleDto);
                        }
                    }
                    await _mediator.Send(new AddArticleDataFromRssFeedCommand()
                    { Articles = list });

                }
            }
            catch (Exception ex)
            {
                Log.Error(ex, "Operation: GetAllArticleDataFromRssAsync was not successful");
                throw;
            }
        }

        private async Task<string> RemoveHtmlTags(string text)
        {
            var input = text.Replace("&nbsp;", " ");
            return Regex.Replace(input, "<.*?>", String.Empty);
        }
    }
}
