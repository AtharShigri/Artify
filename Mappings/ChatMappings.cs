using AutoMapper;
using Artify.Api.DTOs.Shared;
using Artify.Api.Models;
using System.Linq;

namespace Artify.Api.Mappings
{
    public class ChatMappings : Profile
    {
        public ChatMappings()
        {
            CreateMap<Conversation, ConversationDto>()
                .ForMember(dest => dest.ParticipantA_Name, opt => opt.MapFrom(src => src.ParticipantA != null ? src.ParticipantA.FullName : "Unknown"))
                .ForMember(dest => dest.ParticipantB_Name, opt => opt.MapFrom(src => src.ParticipantB != null ? src.ParticipantB.FullName : "Unknown"))
                .ForMember(dest => dest.LastMessage, opt => opt.MapFrom(src => src.Messages.OrderByDescending(m => m.Timestamp).FirstOrDefault().Content ?? ""));

            CreateMap<ChatMessage, MessageDto>()
                .ForMember(dest => dest.SenderName, opt => opt.MapFrom(src => src.Sender != null ? src.Sender.FullName : "Unknown"));

            CreateMap<Notification, NotificationDto>();
        }
    }
}
