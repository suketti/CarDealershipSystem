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
    public partial class EditLocationWindow : Window
    {
        bool isSyncing;
        private LocationDTO location;
        public event EventHandler LocationEdited;

        public EditLocationWindow(LocationDTO location)
        {
            InitializeComponent();
            this.location = location;
            LoadLocationAsync();
            LoadPrefecturesAsync();
        }

        private async void LoadPrefecturesAsync()
        {
            try
            {
                var prefectures = await FetchPrefecturesAsync();
                PrefectureComboBox.ItemsSource = prefectures;
                PrefectureComboBoxJP.ItemsSource = prefectures;

                // Set the prefecture based on names
                if (location.Address.Prefecture != null)
                {
                    PrefectureComboBox.SelectedItem = prefectures.Find(p => p.Name == location.Address.Prefecture.Name);
                    PrefectureComboBoxJP.SelectedItem = prefectures.Find(p => p.NameJP == location.Address.Prefecture.NameJP);
                }
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

        private void LoadLocationAsync()
        {
            try
            {
                if (location != null)
                {
                    LocationNameTextBox.Text = location.LocationName;
                    PostalCodeTextBox.Text = location.Address.PostalCode;
                    CityTextBox.Text = location.Address.City;
                    CityRomanizedTextBox.Text = location.Address.CityRomanized;
                    StreetTextBox.Text = location.Address.Street;
                    StreetRomanizedTextBox.Text = location.Address.StreetRomanized;
                    CapacityTextBox.Text = location.MaxCapacity.ToString();
                    PhoneTextBox.Text = location.PhoneNumber;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading location details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
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
                id = this.location.Id,
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
                var response = await HttpClientService.Client.PutAsync("/api/locations/update", content);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Location edited successfully!");

                LocationEdited?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing location: {ex.Message}");
            }
        }
    }
}