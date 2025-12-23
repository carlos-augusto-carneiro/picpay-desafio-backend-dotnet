namespace Picpay.Infra;

using System.Threading;
using System.Threading.Tasks;
using Picpay.Domain.Repositories;
using Picpay.Infra.Persistence.Context;

public class UnitOfWork : IUnitOfWork
{
    private AppDbContext _context;
    public UnitOfWork(AppDbContext context)
    {
        _context = context;
    }
    public async Task CommitAsync(CancellationToken cancellationToken)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
