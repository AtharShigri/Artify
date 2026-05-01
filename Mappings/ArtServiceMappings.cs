using AutoMapper;
using artifi.Api.DTOs.Artist;
using artifi.Api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace artifi.Api.Mappings
{
    public class ArtServiceMappings : Profile
    {
        public ArtServiceMappings()
        {
            CreateMap<ArtServiceDto, ArtService>();
            CreateMap<ArtService, ArtServiceDto>();
        }
    }
}
