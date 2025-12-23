namespace Picpay.Infra;

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Picpay.Domain.Entities;
using Picpay.Domain.Repositories;
using Picpay.Infra.Persistence.Context;
public class WalletRepository : IWalletRepository
{   
    private readonly AppDbContext _context;
    public WalletRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Wallet wallet)
    {
        await _context.Wallets.AddAsync(wallet);
    }

    public async Task<Wallet?> GetByIdAsync(Guid id)
    {
        return await _context.Wallets
            .Include(w => w.User) 
            .FirstOrDefaultAsync(w => w.Id == id);
    }

    public async Task<Wallet?> GetByUserIdAsync(Guid userId)
    {
        return await _context.Wallets
            .Include(w => w.User)
            .FirstOrDefaultAsync(w => w.UserId == userId);
    }
    
    public void Update(Wallet wallet)
    {
        _context.Wallets.Update(wallet);
    }
}
