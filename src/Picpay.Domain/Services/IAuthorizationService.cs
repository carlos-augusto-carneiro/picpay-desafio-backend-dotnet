namespace Picpay.Domain.Services;

using Picpay.Domain.Entities;
public interface IAuthorizationService
{
    Task<bool> IsAuthorizedAsync(User sender, decimal amount);
}
