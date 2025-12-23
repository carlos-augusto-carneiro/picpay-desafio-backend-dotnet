namespace Picpay.Domain.Repositories;

using Picpay.Domain.Entities;
public interface ITransactionRepository
{
    Task AddAsync(Transaction transaction);
    Task<Transaction?> GetByIdAsync(Guid id);
    Task<ICollection<Transaction>> GetByWalletIdAsync(Guid walletId);
}
