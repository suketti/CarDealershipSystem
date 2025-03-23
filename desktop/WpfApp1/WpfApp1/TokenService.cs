using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Services;

namespace WpfApp1
{
    public class TokenService
    {
        public static async Task<bool> RefreshAccessTokenAsync()
        {
            try
            {
                HttpResponseMessage response = await HttpClientService.Client.PostAsync("api/users/refresh-token", null);
                if (response.IsSuccessStatusCode)
                {
                    // Extract and store new cookies
                    var cookies = response.Headers.GetValues("Set-Cookie");
                    foreach (var cookie in cookies)
                    {
                        var cookieParts = cookie.Split(';')[0].Split('=');
                        if (cookieParts.Length == 2)
                        {
                            HttpClientService.AddCookie(cookieParts[0], cookieParts[1], "your-api-base-url.com");
                        }
                    }
                    return true;
                }
                return false;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Token refresh error: {ex.Message}");
                return false;
            }
        }
    }
}
