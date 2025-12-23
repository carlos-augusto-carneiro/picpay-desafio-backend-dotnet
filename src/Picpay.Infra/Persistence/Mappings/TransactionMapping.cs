namespace Picpay.Infra.Persistence.Mappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Picpay.Domain.Entities;
public class TransactionMapping : IEntityTypeConfiguration<Transaction>
{
    public void Configure(EntityTypeBuilder<Transaction> builder)
    {
        builder.ToTable("Transactions");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property(t => t.Amount)
            .HasColumnName("Amount")
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(t => t.SenderWalletId)
            .HasColumnName("SenderWalletId")
            .IsRequired();

        builder.Property(t => t.ReceiverWalletId)
            .HasColumnName("ReceiverWalletId")
            .IsRequired();

        builder.Property(t => t.CreatedAt)
            .HasColumnName("TransactionDate")
            .IsRequired();

        builder.HasOne(t => t.SenderWallet)
            .WithMany(u => u.SendTransactions)
            .HasForeignKey(t => t.SenderWalletId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(t => t.ReceiverWallet)
            .WithMany(u => u.ReceiveTransactions)
            .HasForeignKey(t => t.ReceiverWalletId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
