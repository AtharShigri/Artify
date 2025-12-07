using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Models;

namespace Artify.Api.Mappings
{
    public class ReviewMappings : Profile
    {
        public ReviewMappings()
        {
            // Review to ReviewResponseDto
            CreateMap<Review, ReviewResponseDto>()
                .ForMember(dest => dest.ReviewId, opt => opt.MapFrom(src => src.ReviewId))
                .ForMember(dest => dest.ReviewerId, opt => opt.MapFrom(src => src.ReviewerId))
                .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
                .ForMember(dest => dest.Comment, opt => opt.MapFrom(src => src.Comment))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.ArtworkId, opt => opt.MapFrom(src => src.ArtworkId))
                .ForMember(dest => dest.ArtistProfileId, opt => opt.MapFrom(src => src.ArtistProfileId));
        }
    }
}