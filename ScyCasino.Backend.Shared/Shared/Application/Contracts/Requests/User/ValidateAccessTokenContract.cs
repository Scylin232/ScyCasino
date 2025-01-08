namespace Shared.Application.Contracts.Requests.User;

public class ValidateAccessTokenContract
{
    public string Issuer { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
}