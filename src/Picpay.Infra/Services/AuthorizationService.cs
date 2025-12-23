namespace Picpay.Infra;

using Picpay.Domain.Services;
using Picpay.Domain.Entities;

public class AuthorizationService : IAuthorizationService
{
    private readonly HttpClient _httpClient;

    public AuthorizationService(IHttpClientFactory httpClientFactory)
    {
        _httpClient = httpClientFactory.CreateClient("AuthorizationApi");
    }

    public async Task<bool> IsAuthorizedAsync(User sender, decimal amount)
    {
        try
        {
            var response = await _httpClient.GetAsync("https://util.devi.tools/api/v2/authorize");

            if (!response.IsSuccessStatusCode)
                return false;

            var content = await response.Content.ReadAsStringAsync();

            return content.Contains("true") || content.Contains("authorization");
        }
        catch
        {
            return false;
        }
    }
}
