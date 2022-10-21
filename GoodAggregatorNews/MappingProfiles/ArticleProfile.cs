using AutoMapper;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Database.Entities;
using GoodAggregatorNews.Models;

namespace GoodAggregatorNews.MappingProfiles
{
    public class ArticleProfile :Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDto>();
            CreateMap<ArticleDto, Article>();   //изменить!
            CreateMap<ArticleDto, ArticleModel>().ReverseMap();

        }
    }
}
