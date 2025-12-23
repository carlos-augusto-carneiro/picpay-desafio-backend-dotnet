using Picpay.Domain.Entities;

namespace Picpay.Domain.Entities;

public class Wallet : BaseEntity
{
    public decimal Balance { get; private set; }

    public Guid UserId { get; private set; }
    public User user { get; private set; } = null!;

    public ICollection<Transaction> SendTransactions { get; private set; }
    public ICollection<Transaction> ReceiveTransactions { get; private set; }

    protected Wallet() { }
    public Wallet(Guid userId)
    {
        UserId = userId;
        Balance = 0;
        SendTransactions = new List<Transaction>();   
        ReceiveTransactions = new List<Transaction>();
    }

    public void Credit(decimal amount)
    {
        Balance += amount;
    }

    public void Debit(decimal amount)
    {   
        if (amount > Balance)
            throw new InvalidOperationException("Insufficient funds.");
        if (amount < 0)
            throw new InvalidOperationException("Amount must be positive.");

        Balance -= amount;
    }
}
