using NotificationService.Application.Abstractions;

namespace NotificationService.Infrastructure.Services;

public class NotificationService : INotificationService
{
    public Task SendAsync(Guid userId, string message)
    {
        Console.WriteLine($"[NOTIFICATION] To: {userId} | Message: {message}");
        
        return Task.CompletedTask;
    }
}