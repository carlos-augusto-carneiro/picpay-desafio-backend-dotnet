namespace Picpay.Domain.Repositories;

public interface IUnitOfWorkRepository
{
    Task CommitAsync(CancellationToken cancellationToken);

}
