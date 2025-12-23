namespace Picpay.Domain.Repositories;

using Picpay.Domain.Entities;
public interface IWalletRepository 
{
    Task<Wallet?> GetByIdAsync(Guid id);
    Task CreditAsync(Guid walletId, decimal amount);
    Task DebitAsync(Guid walletId, decimal amount);
}
