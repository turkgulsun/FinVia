namespace NotificationService.Application.Abstractions;

public interface INotificationService
{
    Task SendAsync(Guid userId, string message);
}