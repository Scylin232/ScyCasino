using System.Net.Http.Headers;
using System.Text.Json;
using Domain.Services;
using Infrastructure.DTO;

namespace Infrastructure.Services;

public class UserService(HttpClient httpClient) : IUserService
{
    public async Task<Guid?> GetUserIdByToken(string token)
    {
        using HttpRequestMessage requestMessage = new(HttpMethod.Get, $"http://user-service:7231/api/user/self");
        requestMessage.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        HttpResponseMessage response = await httpClient.SendAsync(requestMessage);
        
        if (!response.IsSuccessStatusCode) return null;
        
        string content = response.Content.ReadAsStringAsync().Result;
        UserResponse? user = JsonSerializer.Deserialize<UserResponse>(content);
        
        if (user is null || user.Id == Guid.Empty) return null;
        
        return user.Id;
    }
}