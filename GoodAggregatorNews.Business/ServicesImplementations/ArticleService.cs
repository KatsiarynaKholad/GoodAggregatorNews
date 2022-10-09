using AutoMapper;
using GoodAggregatorNews.Abstractions;
using GoodAggregatorNews.Core;
using GoodAggregatorNews.Core.Abstractions;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Database.Entities;
using Microsoft.EntityFrameworkCore;
using Serilog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GoodAggregatorNews.Business.ServicesImplementations
{
    public class ArticleService : IArticleService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public ArticleService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
        }

        public async Task<int> CreateArticleAsync(ArticleDto dto)
        {
            try
            {
                var entity = _mapper.Map<Article>(dto);
                if (entity!=null)
                {
                    await _unitOfWork.Articles.AddAsync(entity);
                    var resultSaveChanges = await _unitOfWork.Commit();
                    return resultSaveChanges;
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
                var entity = await _unitOfWork.Articles.GetByIdAsync(id);
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

        public async Task<List<ArticleDto>> GetArticlesByPageNumberAndPageSizeAsync(int pageNumber, int pageSize)
        {
            try
            {
                var list = await _unitOfWork.Articles
                    .Get()
                    .Skip(pageSize * pageNumber)
                    .Take(pageSize)
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

        public async Task<List<ArticleDto>> GetNewArticlesFromExternalSourcesAsync()    //переписать
        {
            var list = new List<ArticleDto>();
            return list;
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
    }
}
