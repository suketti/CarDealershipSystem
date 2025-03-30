using System.Net.Http;
using System.Net;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System;
using WpfApp1.Models;
using WpfApp1.Services;

public static class HttpClientService
{
    private static readonly CookieContainer cookieContainer;
    private static readonly HttpClientHandler handler;
    private static readonly HttpClient client;

    static HttpClientService()
    {
        cookieContainer = new CookieContainer();

        handler = new HttpClientHandler
        {
            CookieContainer = cookieContainer,
            ServerCertificateCustomValidationCallback = (request, certificate, chain, sslPolicyErrors) => true
        };

        // Combine the handlers: AuthenticatedHttpClientHandler + TokenRefreshHttpHandler
        var authenticatedHandler = new AuthenticatedHttpClientHandler(handler);
        var tokenRefreshHandler = new TokenRefreshHttpHandler(authenticatedHandler);

        client = new HttpClient(tokenRefreshHandler)
        {
            BaseAddress = new Uri("https://192.168.1.100:7268")
        };
    }

    public static HttpClient Client => client;

    public static void AddCookie(string name, string value, string domain)
    {
        var uri = new Uri(domain);
        var host = uri.Host;
        cookieContainer.Add(new Cookie(name, value, "/", host));
    }

    public static CookieContainer CookieContainer => cookieContainer;

    // Method to refresh the access token using the refresh token
    public static async Task<bool> RefreshTokenIfNeeded()
    {
        var cookies = cookieContainer.GetCookies(client.BaseAddress);
        var refreshToken = cookies["RefreshToken"]?.Value;

        if (refreshToken != null)
        {
            var content = new StringContent($"\"{refreshToken}\"", Encoding.UTF8, "application/json");
            HttpResponseMessage response = await client.PostAsync("/api/users/refresh", content);

            if (response.IsSuccessStatusCode)
            {
                var responseContent = await response.Content.ReadAsStringAsync();
                var newTokens = JsonSerializer.Deserialize<TokenResponseDTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (newTokens?.AccessToken != null)
                {
                    AddCookie("AccessToken", newTokens.AccessToken, client.BaseAddress.ToString());
                    AddCookie("RefreshToken", newTokens.RefreshToken, client.BaseAddress.ToString());
                    return true;
                }
            }
        }

        return false;
    }

    public static string GetAccessToken()
    {
        var cookies = cookieContainer.GetCookies(client.BaseAddress);
        return cookies["AccessToken"]?.Value;
    }
}
