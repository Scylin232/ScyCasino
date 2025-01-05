using System.Text;
using System.Security.Cryptography;

namespace Application.Utilities.User;

public static class UserUtilities
{
    public static Guid GenerateGuidFromClaims(string issuer, string subject)
    {
        string combinedClaims = $"{subject}|{issuer}";
        
        using SHA256 sha256Hash = SHA256.Create();
        byte[] hashBytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(combinedClaims));
        
        return new Guid(hashBytes[..16]);
    }
}