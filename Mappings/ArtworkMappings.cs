using AutoMapper;
using artifi.Api.DTOs.Artist;
using artifi.Api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace artifi.Api.Mappings
{
    public class ArtworkMappings : Profile
    {
        public ArtworkMappings()
        {
            CreateMap<ArtworkUploadDto, Artwork>();
            CreateMap<ArtworkUpdateDto, Artwork>();
            CreateMap<Artwork, ArtworkUpdateDto>();
        }
    }
}
