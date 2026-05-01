using artifi.Api.DTOs.Shared;
using artifi.Api.Models;
using artifi.Api.Repositories.Interfaces;
using artifi.Api.Services.Interfaces;
using artifi.Api.Hubs;
using AutoMapper;
using Microsoft.AspNetCore.SignalR;

namespace artifi.Api.Services.Implementations
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepo;
        private readonly IMapper _mapper;
        private readonly IHubContext<NotificationHub> _hubContext;

        public NotificationService(
            INotificationRepository notificationRepo,
            IMapper mapper,
            IHubContext<NotificationHub> hubContext)
        {
            _notificationRepo = notificationRepo;
            _mapper = mapper;
            _hubContext = hubContext;
        }

        public async Task<NotificationDto> SendNotificationAsync(Guid userId, string title, string message, string type = "Info", string? redirectUrl = null)
        {
            var notification = new Notification
            {
                UserId = userId,
                Title = title,
                Message = message,
                Type = type,
                RedirectUrl = redirectUrl,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            var saved = await _notificationRepo.AddNotificationAsync(notification);
            var dto = _mapper.Map<NotificationDto>(saved);

            // Emit via SignalR
            await _hubContext.Clients.Group($"User_{userId}").SendAsync("ReceiveNotification", dto);

            return dto;
        }

        public async Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(Guid userId, int count = 20)
        {
            var notifications = await _notificationRepo.GetUserNotificationsAsync(userId, count);
            return _mapper.Map<IEnumerable<NotificationDto>>(notifications);
        }

        public async Task<int> GetUnreadCountAsync(Guid userId)
        {
            return await _notificationRepo.GetUnreadCountAsync(userId);
        }

        public async Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId)
        {
            return await _notificationRepo.MarkAsReadAsync(notificationId, userId);
        }

        public async Task<bool> MarkAllAsReadAsync(Guid userId)
        {
            return await _notificationRepo.MarkAllAsReadAsync(userId);
        }
    }
}
