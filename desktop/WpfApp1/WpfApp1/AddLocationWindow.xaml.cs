using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1
{
    public partial class AddLocationWindow : Window
    {
        bool isSyncing;
        public event EventHandler LocationAdded;
        public AddLocationWindow()
        {
            InitializeComponent();
            LoadPrefecturesAsync();
        }

        private async void LoadPrefecturesAsync()
        {
            try
            {
                var prefectures = await FetchPrefecturesAsync();
                PrefectureComboBox.ItemsSource = prefectures;
                PrefectureComboBoxJP.ItemsSource = prefectures;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching prefectures: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<List<PrefectureDTO>> FetchPrefecturesAsync()
        {
            HttpResponseMessage response = await HttpClientService.Client.GetAsync("/api/locations/prefectures");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<PrefectureDTO>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private void PrefectureComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSyncing) return;

            isSyncing = true;

            int selectedIndex = PrefectureComboBox.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < PrefectureComboBoxJP.Items.Count)
            {
                PrefectureComboBoxJP.SelectedIndex = selectedIndex;
            }

            isSyncing = false;
        }

        private void PrefectureComboBoxJP_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (isSyncing) return;

            isSyncing = true;

            int selectedIndex = PrefectureComboBoxJP.SelectedIndex;
            if (selectedIndex >= 0 && selectedIndex < PrefectureComboBox.Items.Count)
            {
                PrefectureComboBox.SelectedIndex = selectedIndex;
            }

            isSyncing = false;
        }


        private async void Save_Click(object sender, RoutedEventArgs e)
        {
            var location = new
            {
                id = 0,
                locationName = LocationNameTextBox.Text,
                address = new
                {
                    postalCode = PostalCodeTextBox.Text,
                    prefecture = new
                    {
                        name = ((PrefectureDTO)PrefectureComboBox.SelectedItem)?.Name,
                        nameJP = ((PrefectureDTO)PrefectureComboBox.SelectedItem)?.NameJP
                    },
                    city = CityTextBox.Text,
                    cityRomanized = CityRomanizedTextBox.Text,
                    street = StreetTextBox.Text,
                    streetRomanized = StreetRomanizedTextBox.Text
                },
                maxCapacity = int.Parse(CapacityTextBox.Text),
                phoneNumber = PhoneTextBox.Text
            };

            var jsonContent = JsonSerializer.Serialize(location);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await HttpClientService.Client.PostAsync("/api/locations", content);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Location added successfully!");

                LocationAdded?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding location: {ex.Message}");
            }
        }
    }
}