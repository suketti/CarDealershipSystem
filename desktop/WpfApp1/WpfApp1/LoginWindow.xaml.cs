using System;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
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
        private UserDTO _loggedInUser;

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

            UserDTO user = await LoginAsync(email, password);
            if (user != null)
            {
                MessageBox.Show($"Login successful! Welcome, {user.Name}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                MainWindow mainWindow = new MainWindow(user);
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Invalid email or password!", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
   

    private async Task<UserDTO> LoginAsync(string email, string password)
        {
            var loginDto = new { Email = email, Password = password };
            string json = JsonSerializer.Serialize(loginDto);
            HttpContent content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                HttpResponseMessage response = await HttpClientService.Client.PostAsync("api/users/login", content);
                if (response.IsSuccessStatusCode)
                {
                    string responseBody = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<UserDTO>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                return null;
            }
            catch (HttpRequestException ex)
            {
                MessageBox.Show($"Request error: {ex.Message}");
                return null;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"General error: {ex.Message}");
                return null;
            }
        }

    }
}
