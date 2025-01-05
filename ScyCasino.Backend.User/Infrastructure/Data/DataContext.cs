using Microsoft.EntityFrameworkCore;
using Domain.Models;
using SharedKernel.Repositories;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options), IUnitOfWork
{
    public DbSet<User> Users { get; set; }
    
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default) =>
        await base.SaveChangesAsync(cancellationToken);
}