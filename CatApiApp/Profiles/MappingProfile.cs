using AutoMapper;
using CatApiApp.Dtos;
using CatApiApp.Models;

namespace CatApiApp.Profiles {
    /// <summary>
    /// Profile configuration for mapping between domain entities and DTOs using AutoMapper.
    /// </summary>
    public class MappingProfile : Profile {

        /// <summary>
        /// Initializes a new instance of the <see cref="MappingProfile"/> class.
        /// </summary>
        public MappingProfile() {
            CreateMap<CatEntity, CatDto>()
                .ForMember(dest => dest.Tags, opt => opt.MapFrom(src => src.Tags.Select(t => t.Name)));
        }
    }
}
