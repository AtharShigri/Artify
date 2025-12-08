using AutoMapper;
using Artify.Api.DTOs.Artist;
using Artify.Api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Artify.Api.Mappings
{
    public class ArtistMappings : Profile
    {
        public ArtistMappings()
        {
            CreateMap<ArtistRegisterDto, Artist>();
            CreateMap<ArtistUpdateDto, Artist>();
            CreateMap<Artist, ArtistUpdateDto>();
        }
    }
}
