using System;
using System.Collections;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private async void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailTextBox.Text;
            string password = PasswordBox.Password;

            if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(password))
            {
                MessageBox.Show("Kérlek töltsd ki az összes mezőt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            bool loginSuccess = await LoginAsync(email, password);
            if (loginSuccess)
            {
                // Extract cookies
                var cookieContainer = HttpClientService.CookieContainer;
                var cookies = cookieContainer.GetCookies(HttpClientService.Client.BaseAddress);

                string accessToken = cookies["AccessToken"]?.Value;
                string refreshToken = cookies["RefreshToken"]?.Value;
                string userId = cookies["userId"]?.Value;

                if (accessToken != null && refreshToken != null && userId != null)
                {
                    MessageBox.Show("Sikeres bejelentkezés!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow mainWindow = new MainWindow(accessToken, refreshToken, userId, HttpClientService.Client.BaseAddress.ToString());
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Bejelentkezés sikertelen: Hiányzó tokenek.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Érvénytelen e-mail vagy jelszó!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<bool> LoginAsync(string email, string password)
        {
            var loginDto = new
            {
                Email = email,
                Password = password
            };
            string json = JsonSerializer.Serialize(loginDto);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                // Call the new dealer-login endpoint
                HttpResponseMessage response = await HttpClientService.Client.PostAsync("/api/users/dealer-login", content);

                // Check if the login was successful
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response
                    var responseContent = await response.Content.ReadAsStringAsync();
                    var loginResult = JsonSerializer.Deserialize<DealerLoginResponseDTO>(responseContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    if (loginResult != null && loginResult.User != null)
                    {
                        return true;
                    }
                }

                return false;  // Return false if the login fails
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Kérési hiba: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }
    }
}
