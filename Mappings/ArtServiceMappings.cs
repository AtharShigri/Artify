using AutoMapper;
using Artify.Api.DTOs.Artist;
using Artify.Api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Artify.Api.Mappings
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
