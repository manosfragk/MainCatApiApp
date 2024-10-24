using AutoMapper;
using CatApiApp.Dtos;
using CatApiApp.Models;

namespace CatApiApp.Profiles {
    public class MappingProfile : Profile {
        public MappingProfile() {
            CreateMap<CatEntity, CatDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name)));
        }
    }
}
