namespace Picpay.Infra.Repositories;

using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Picpay.Domain.Entities;
using Picpay.Domain.Repositories;
using Picpay.Infra.Persistence.Context;

public class UserRepository : IUserRepository
{
    private readonly AppDbContext _context;
    public UserRepository(AppDbContext context)
    {
        _context = context;
    }
    public async Task AddAsync(User user)
    {
        await _context.Users.AddAsync(user);
    }

    public async Task<User?> GetByDocumentAsync(string document)
    {
        return await _context.Users
            .FirstOrDefaultAsync(u => u.Document == document);
    }

    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
    }

    public async Task<User?> GetByIdAsync(Guid id)
    {
        return await _context.Users
            .Include(u => u.Wallet)
            .FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task UpdateAsync(User user)
    {
        _context.Users.Update(user);
        await Task.CompletedTask;
    }
}
