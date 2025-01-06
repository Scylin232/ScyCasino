using System.Text.Json.Serialization;

namespace Infrastructure.DTO;

public class UserResponse
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
}