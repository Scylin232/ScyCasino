﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Infrastructure.Data;

public class DateContextFactory : IDesignTimeDbContextFactory<DataContext>
{
    public DataContext CreateDbContext(string[] args)
    {
        DbContextOptionsBuilder<DataContext> optionsBuilder = new();

        optionsBuilder.UseNpgsql();

        return new DataContext(optionsBuilder.Options);
    }
}