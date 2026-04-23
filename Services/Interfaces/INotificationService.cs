using Artify.Api.DTOs.Shared;

namespace Artify.Api.Services.Interfaces
{
    public interface INotificationService
    {
        Task<NotificationDto> SendNotificationAsync(Guid userId, string title, string message, string type = "Info", string? redirectUrl = null);
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(Guid userId, int count = 20);
        Task<int> GetUnreadCountAsync(Guid userId);
        Task<bool> MarkAsReadAsync(Guid notificationId, Guid userId);
        Task<bool> MarkAllAsReadAsync(Guid userId);
    }
}
