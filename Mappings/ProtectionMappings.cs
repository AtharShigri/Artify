using AutoMapper;
using Artify.Api.DTOs.Artist;
using Artify.Api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Artify.Api.Mappings
{
    public class ProtectionMappings : Profile
    {
        public ProtectionMappings()
        {
            CreateMap<MetadataDto, ArtworkMetadataLog>();
            CreateMap<HashDto, ArtworkHash>();
            CreateMap<WatermarkResponseDto, Artwork>();
        }
    }
}
