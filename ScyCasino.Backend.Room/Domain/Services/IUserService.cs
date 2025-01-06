namespace Domain.Services;

public interface IUserService
{
    Task<Guid?> GetUserIdByToken(string token);
}