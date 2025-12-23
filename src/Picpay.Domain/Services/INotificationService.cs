namespace Picpay.Domain.Services;
using Picpay.Domain.Entities;

public interface INotificationService
{
    Task SendNotificationAsync(User user, string message);
}
