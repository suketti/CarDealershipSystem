using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Text.Json;
using System.Windows.Input;
using WpfApp1.Services;
using WpfApp1.Views;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly string _accessToken;
        private readonly string _refreshToken;
        private readonly string _userId;
        public List<string> UserPrivileges { get; private set; } = new List<string>();

        public MainWindow(string accessToken, string refreshToken, string userId, string domain)
        {
            InitializeComponent();

            _accessToken = accessToken;
            _refreshToken = refreshToken;
            _userId = userId;

            // Store cookies for future authenticated requests
            HttpClientService.AddCookie("AccessToken", accessToken, domain);
            HttpClientService.AddCookie("RefreshToken", refreshToken, domain);
            HttpClientService.AddCookie("userId", userId, domain);

            // Navigate to the default page
            MainFrame.Navigate(new CarsPage());
            tcPages.SelectedIndex = 0;

            // Subscribe to the Loaded event
            this.Loaded += MainWindow_Loaded;
        }

        private async void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            // Now call the async method after the window is loaded
            await InitializeAsync();
        }

        private async Task InitializeAsync()
        {
            // Check if the user is an admin
            if (await IsUserAdmin(Guid.Parse(_userId)))
            {
                var adminTab = new TabItem
                {
                    Header = "Admin Panel",
                    Cursor = Cursors.Hand,
                    Content = new TextBlock
                    {
                        Text = "Admin Panel Content",
                        HorizontalAlignment = HorizontalAlignment.Center,
                        VerticalAlignment = VerticalAlignment.Center
                    }
                };

                // Add the Admin tab to the TabControl
                tcPages.Items.Add(adminTab);
            }
        }

        private void TabControl_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (MainFrame == null) return;

            switch (tcPages.SelectedIndex)
            {
                case 0:
                    MainFrame.Navigate(new CarsPage());
                    break;
                case 1:
                    MainFrame.Navigate(new LocationsPage());
                    break;
                case 2:
                    MainFrame.Navigate(new CarMakersPage());
                    break;
                case 3:
                    MainFrame.Navigate(new CarModelPage());
                    break;
                case 4:
                    MainFrame.Navigate(new UserPage());
                    break;
            }
        }

        private async Task<bool> IsUserAdmin(Guid userId)
        {
            HttpResponseMessage response = await HttpClientService.Client.GetAsync($"/api/users/getPrivilege?userId={userId}");

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            string jsonString = await response.Content.ReadAsStringAsync(); // Read JSON as a string
            var privileges = JsonSerializer.Deserialize<List<string>>(jsonString); // Deserialize manually

            if (privileges == null || privileges.Count == 0)
            {
                return false;
            }

            UserPrivileges = privileges; // Store privileges

            return privileges.Contains("Admin");
        }
    }
}
