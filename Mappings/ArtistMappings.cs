using AutoMapper;
using artifi.Api.DTOs.Artist;
using artifi.Api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace artifi.Api.Mappings
{
    public class ArtistMappings : Profile
    {
        public ArtistMappings()
        {
            CreateMap<ArtistRegisterDto, ApplicationUser>();
            CreateMap<ArtistUpdateDto, ApplicationUser>();
            CreateMap<ApplicationUser, ArtistUpdateDto>();
        }
    }
}
