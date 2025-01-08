using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Data;

namespace Infrastructure.Data;

public class DataContext(DbContextOptions<DataContext> options) : EntityFrameworkDataContext(options)
{
    public DbSet<RouletteBet> RouletteBets { get; set; }
    public DbSet<RouletteGameState> RouletteGameStates { get; set; }
}