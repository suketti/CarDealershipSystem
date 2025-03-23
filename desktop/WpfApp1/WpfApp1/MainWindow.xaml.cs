
using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Services;
using WpfApp1.Views;

namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        private readonly string _accessToken;
        private readonly string _refreshToken;
        private readonly string _userId;

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

            MainFrame.Navigate(new CarsPage());
            tcPages.SelectedIndex = 0;
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
            }
        }
    }
}
