using AutoMapper;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Database.Entities;
using GoodAggregatorNews.Models;

namespace GoodAggregatorNews.WebAPI.MappingProfiles
{
    public class SourceProfile : Profile
    {
        public SourceProfile()
        {
            CreateMap<Source, SourceDto>();
            CreateMap<SourceDto, Source>();

            CreateMap<SourceDto, SourceModel>();

            CreateMap<CreateSourceModel, SourceDto>()
                .ForMember(dto => dto.Id,
                opt =>
                opt.MapFrom(art => Guid.NewGuid()));
        }
    }
}
