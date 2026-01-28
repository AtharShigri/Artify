using AutoMapper;
using Artify.Api.DTOs.Artist;
using Artify.Api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Artify.Api.Mappings
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
