using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Models;

namespace Artify.Api.Mappings
{
    public class BuyerMappings : Profile
    {
        public BuyerMappings()
        {
            // ApplicationUser to BuyerProfileResponseDto
            CreateMap<ApplicationUser, BuyerProfileResponseDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.FullName, opt => opt.MapFrom(src => src.FullName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.ProfileImageUrl, opt => opt.MapFrom(src => src.ProfileImageUrl))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt));
        }
    }
}