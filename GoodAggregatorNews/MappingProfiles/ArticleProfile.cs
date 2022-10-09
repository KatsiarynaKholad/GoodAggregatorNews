using AutoMapper;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Database.Entities;

namespace GoodAggregatorNews.MappingProfiles
{
    public class ArticleProfile :Profile
    {
        public ArticleProfile()
        {
            CreateMap<Article, ArticleDto>();
            CreateMap<ArticleDto, Article>();   //изменить!
        }
    }
}
