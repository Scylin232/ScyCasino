using Microsoft.EntityFrameworkCore;
using Shared.Kernel.Repositories;

namespace Shared.Infrastructure.Data;

public class EntityFrameworkDataContext(DbContextOptions options) : DbContext(options), IUnitOfWork
{
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await base.SaveChangesAsync(cancellationToken);
}