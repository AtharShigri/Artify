using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Models;

namespace Artify.Api.Mappings
{
    public class HiringMappings : Profile
    {
        public HiringMappings()
        {
            // Order (as Hiring) to HiringResponseDto
            CreateMap<Order, HiringResponseDto>()
                .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.BuyerId, opt => opt.MapFrom(src => src.BuyerId))
                .ForMember(dest => dest.ArtistProfileId, opt => opt.MapFrom(src => src.ArtistProfileId))
                .ForMember(dest => dest.Budget, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.DeliveryStatus))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => src.CreatedAt))
                .ForMember(dest => dest.Deadline, opt => opt.MapFrom(src => src.CompletionDate));
        }
    }
}