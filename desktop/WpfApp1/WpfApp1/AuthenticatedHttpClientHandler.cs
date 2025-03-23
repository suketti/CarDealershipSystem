using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Net;

namespace WpfApp1.Services
{
    public class AuthenticatedHttpClientHandler : DelegatingHandler
    {
        public AuthenticatedHttpClientHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            // Automatically add AccessToken from cookie (if exists)
            var cookieContainer = HttpClientService.CookieContainer;
            var cookies = cookieContainer.GetCookies(request.RequestUri);
            var accessTokenCookie = cookies["AccessToken"];

            if (accessTokenCookie != null)
            {
                request.Headers.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessTokenCookie.Value);
            }

            return base.SendAsync(request, cancellationToken);
        }
    }
}
