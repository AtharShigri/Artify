using AutoMapper;
using artifi.Api.DTOs.Artist;
using artifi.Api.Models;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace artifi.Api.Mappings
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
