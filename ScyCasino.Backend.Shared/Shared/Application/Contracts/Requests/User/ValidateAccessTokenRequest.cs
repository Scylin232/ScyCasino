namespace Shared.Application.Contracts.Requests.User;

public class ValidateAccessTokenRequest
{
    public string Issuer { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
}

public class ValidateAccessTokenResponse
{
    public Guid? UserId { get; set; }
    public bool IsValid { get; set; }
}