using Microsoft.EntityFrameworkCore;
using Domain.Models;
using Shared.Infrastructure.Data;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : EntityFrameworkDataContext(options)
{
    public DbSet<User> Users { get; set; }
}