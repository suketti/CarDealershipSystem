using System;
using System.Net;
using System.Net.Http;

namespace WpfApp1.Services
{
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

            var authenticatedHandler = new AuthenticatedHttpClientHandler(handler);
            client = new HttpClient(authenticatedHandler)
            {
                BaseAddress = new Uri("https://172.20.10.2:7268")
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
    }
}
