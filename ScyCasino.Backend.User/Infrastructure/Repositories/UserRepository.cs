using System.Linq.Expressions;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Shared.Infrastructure.Repositories;

namespace Infrastructure.Repositories;

public class UserRepository(DataContext dataContext) : EntityFrameworkRepository<User>(dataContext), IUserRepository
{
    public async Task<List<User>> GetUsersByIds(IEnumerable<Guid> ids)
    {
        IQueryable<User> usersQuery = dataContext.Set<User>().AsNoTracking();
        
        return await usersQuery
            .Where(user => ids.Contains(user.Id))
            .ToListAsync();
    }
    
    protected override Expression<Func<User, object>> GetOrderingProperty(string? sortColumn)
    {
        Expression<Func<User, object>> orderingProperty = sortColumn switch
        {
            "coins" => user => user.Coins,
            _ => user => user.Id
        };
        
        return orderingProperty;
    }
}