namespace Picpay.Infra.Persistence.Mappings;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Picpay.Domain.Entities;
public class WalletMapping : IEntityTypeConfiguration<Wallet>
{
    public void Configure(EntityTypeBuilder<Wallet> builder)
    {
        builder.ToTable("Wallets");

        builder.HasKey(w => w.Id);

        builder.Property(w => w.Id)
            .HasColumnName("Id")
            .IsRequired();

        builder.Property(w => w.Balance)
            .HasColumnName("Balance")
            .IsRequired()
            .HasColumnType("decimal(18,2)");

        builder.Property(w => w.UserId)
            .HasColumnName("UserId")
            .IsRequired();

        builder.HasOne(w => w.User)
            .WithOne(u => u.Wallet)
            .HasForeignKey<Wallet>(w => w.UserId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
