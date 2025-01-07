using System.Linq.Expressions;
using Domain.Models;
using Domain.Repositories;
using Infrastructure.Data;
using Shared.Infrastructure.Repositories;

namespace Infrastructure.Repositories;

public class UserRepository(DataContext dataContext) : EntityFrameworkRepository<User>(dataContext), IUserRepository
{
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