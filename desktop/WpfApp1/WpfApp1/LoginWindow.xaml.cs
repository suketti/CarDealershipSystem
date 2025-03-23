using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
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
                MessageBox.Show("Please fill in all fields.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
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
                    MessageBox.Show($"Login successful!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    MainWindow mainWindow = new MainWindow(accessToken, refreshToken, userId, HttpClientService.Client.BaseAddress.ToString());
                    mainWindow.Show();
                    Close();
                }
                else
                {
                    MessageBox.Show("Login failed: Missing tokens.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("Invalid email or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<bool> LoginAsync(string email, string password)
        {
            var loginDto = new { Email = email, Password = password };
            string json = JsonSerializer.Serialize(loginDto);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await HttpClientService.Client.PostAsync("/api/users/login", content);
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}");
                return false;
            }
        }
    }
}