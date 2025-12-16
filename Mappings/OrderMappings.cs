using AutoMapper;
using Artify.Api.DTOs.Buyer;
using Artify.Api.Models;

namespace Artify.Api.Mappings
{
    public class OrderMappings : Profile
    {
        public OrderMappings()
        {
            // Order to OrderResponseDto (basic mapping)
            CreateMap<Order, OrderResponseDto>()
                .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderId))
                .ForMember(dest => dest.BuyerId, opt => opt.MapFrom(src => src.BuyerId))
                .ForMember(dest => dest.TotalAmount, opt => opt.MapFrom(src => src.TotalAmount))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus))
                .ForMember(dest => dest.OrderType, opt => opt.MapFrom(src => src.OrderType))
                .ForMember(dest => dest.DeliveryStatus, opt => opt.MapFrom(src => src.DeliveryStatus))
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => src.OrderDate))
                .ForMember(dest => dest.CompletionDate, opt => opt.MapFrom(src => src.CompletionDate));
        }
    }
}