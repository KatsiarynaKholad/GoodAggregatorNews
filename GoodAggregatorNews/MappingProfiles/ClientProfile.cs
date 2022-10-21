using AutoMapper;
using GoodAggregatorNews.Core.DataTransferObject;
using GoodAggregatorNews.Database.Entities;
using GoodAggregatorNews.Models;

namespace GoodAggregatorNews.MappingProfiles
{
    public class ClientProfile : Profile
    {
        public ClientProfile()
        {
            CreateMap<Client, ClientDto>().ForMember(dto => dto.RoleName,
             opt
              => opt.MapFrom(entity => entity.Role.Name));

            CreateMap<ClientDto, Client>()
                .ForMember(ent => ent.Id,
                    opt
                        => opt.MapFrom(dto => Guid.NewGuid()))
                .ForMember(ent => ent.RegistationDate,
                    opt
                        => opt.MapFrom(dto => DateTime.Now));

            CreateMap<RegisterModel, ClientDto>();

            CreateMap<LoginModel, ClientDto>();

            CreateMap<ClientDto, ClientDataModel>();
        }
    }
}
