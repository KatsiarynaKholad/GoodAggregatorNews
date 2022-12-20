using AutoMapper;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Database.Entities;
using GoodAggregatorNews.Models;

namespace GoodAggregatorNews.MappingProfiles
{
    public class CommentProfile  : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>();
            CreateMap<CommentDto, Comment>();
        }
    }
}
