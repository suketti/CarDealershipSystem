using System.Diagnostics;
using System.Net.Http;
using System.Net;
using System.Threading.Tasks;
using System.Threading;

public class TokenRefreshHttpHandler : DelegatingHandler
{
    public TokenRefreshHttpHandler(HttpMessageHandler innerHandler) : base(innerHandler) { }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        // Log the request being sent
        Debug.WriteLine("Sending request: " + request.Method + " " + request.RequestUri);

        // Attempt to send the request
        HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

        // Log the response status code
        Debug.WriteLine("Response Status: " + response.StatusCode);

        // If the response is 401 Unauthorized, attempt to refresh the token
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            Debug.WriteLine("Token expired, attempting to refresh...");

            // Try to refresh the token
            bool refreshSuccess = await HttpClientService.RefreshTokenIfNeeded();

            if (refreshSuccess)
            {
                var newAccessToken = HttpClientService.GetAccessToken();
                if (!string.IsNullOrEmpty(newAccessToken))
                {
                    Debug.WriteLine("Access token refreshed, retrying request with new token.");

                    // Update the authorization header with the new access token
                    request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", newAccessToken);

                    // Retry the original request with the new access token
                    response = await base.SendAsync(request, cancellationToken);
                }
                else
                {
                    Debug.WriteLine("Failed to get new access token.");
                }
            }
            else
            {
                Debug.WriteLine("Token refresh failed.");
            }
        }

        return response;
    }
}
