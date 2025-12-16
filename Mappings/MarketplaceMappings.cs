using AutoMapper;
using Artify.Api.DTOs.Shared;
using Artify.Api.Models;

namespace Artify.Api.Mappings
{
    public class MarketplaceMappings : Profile
    {
        public MarketplaceMappings()
        {
            // Artwork to ArtworkResponseDto
            CreateMap<Artwork, ArtworkResponseDto>()
                .ForMember(dest => dest.ArtworkId, opt => opt.MapFrom(src => src.ArtworkId))
                .ForMember(dest => dest.Title, opt => opt.MapFrom(src => src.Title))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => src.Price))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.ArtistName, opt => opt.MapFrom(src =>
                    src.ArtistProfile != null && src.ArtistProfile.User != null
                        ? src.ArtistProfile.User.FullName
                        : "Unknown Artist"))
                .ForMember(dest => dest.ArtistProfileId, opt => opt.MapFrom(src => src.ArtistProfileId))
                .ForMember(dest => dest.LikesCount, opt => opt.MapFrom(src => src.LikesCount))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.IsForSale, opt => opt.MapFrom(src => src.IsForSale))
                .ForMember(dest => dest.Stock, opt => opt.MapFrom(src => src.Stock));

            // Artwork to ArtworkDetailDto (includes additional fields)
            CreateMap<Artwork, ArtworkDetailDto>()
                .IncludeBase<Artwork, ArtworkResponseDto>()
                .ForMember(dest => dest.ArtistBio, opt => opt.MapFrom(src =>
                    src.ArtistProfile != null ? src.ArtistProfile.Bio : ""))
                .ForMember(dest => dest.ArtistLocation, opt => opt.MapFrom(src =>
                    src.ArtistProfile != null ? src.ArtistProfile.Location : ""))
                .ForMember(dest => dest.ArtistProfileImage, opt => opt.MapFrom(src =>
                    src.ArtistProfile != null ? src.ArtistProfile.ProfileImageUrl : ""))
                .ForMember(dest => dest.ArtistSkills, opt => opt.MapFrom(src =>
                    src.ArtistProfile != null && !string.IsNullOrEmpty(src.ArtistProfile.Skills)
                        ? src.ArtistProfile.Skills.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim()).ToList()
                        : new List<string>()))
                .ForMember(dest => dest.ViewsCount, opt => opt.MapFrom(src => src.ViewsCount));

            // ArtistProfile to ArtistProfileDto
            CreateMap<ArtistProfile, ArtistProfileDto>()
                .ForMember(dest => dest.ArtistProfileId, opt => opt.MapFrom(src => src.ArtistProfileId))
                .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
                    src.User != null ? src.User.FullName : ""))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.Category, opt => opt.MapFrom(src => src.Category))
                .ForMember(dest => dest.Location, opt => opt.MapFrom(src => src.Location))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.ProfileImageUrl))
                .ForMember(dest => dest.PortfolioUrl, opt => opt.MapFrom(src => src.PortfolioUrl))
                .ForMember(dest => dest.Skills, opt => opt.MapFrom(src =>
                    !string.IsNullOrEmpty(src.Skills)
                        ? src.Skills.Split(',', StringSplitOptions.RemoveEmptyEntries)
                            .Select(s => s.Trim()).ToList()
                        : new List<string>()))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}