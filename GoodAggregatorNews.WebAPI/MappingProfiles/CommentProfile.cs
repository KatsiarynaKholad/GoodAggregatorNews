using AutoMapper;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Database.Entities;

namespace GoodAggregatorNews.WebAPI.MappingProfiles
{
    public class CommentProfile : Profile
    {
        public CommentProfile()
        {
            CreateMap<Comment, CommentDto>();


            CreateMap<CommentDto, Comment>();

        }
    }
}
