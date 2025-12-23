namespace Picpay.Domain.Entities;

public class Transaction : BaseEntity
{
    public decimal Amount { get; private set; }

    public Guid SenderWalletId { get; private set; }
    public Wallet SenderWallet { get; private set; } = null!;

    public Guid ReceiverWalletId { get; private set; }
    public Wallet ReceiverWallet { get; private set; } = null!;

    public Transaction(decimal amount, Wallet senderWallet,  Wallet receiverWallet)
    {
        if (amount <= 0)
            throw new InvalidOperationException("Amount must be positive.");
        Amount = amount;
        SenderWallet = senderWallet;
        SenderWalletId = senderWallet.Id;
        ReceiverWallet = receiverWallet;
        ReceiverWalletId = receiverWallet.Id;
    }

}
