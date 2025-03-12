using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace WpfApp1.Services
{
    public class HttpClientService
    {
        private static readonly HttpClientHandler handler;
        private static readonly HttpClient client;

        static HttpClientService()
        {
            handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (req, cert, chain, errors) => true
            };
            client = new HttpClient(handler)
            {
                BaseAddress = new Uri("https://172.20.10.2:7268")
            };
        }

        public static HttpClient Client => client;
    }
}
