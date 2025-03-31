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
                MessageBox.Show($"Hiba a prefektúrák lekérésekor: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
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
            // Validate inputs
            if (string.IsNullOrWhiteSpace(LocationNameTextBox.Text))
            {
                MessageBox.Show("A helyszín neve kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (PrefectureComboBox.SelectedItem == null)
            {
                MessageBox.Show("A prefektúra kiválasztása kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(PostalCodeTextBox.Text))
            {
                MessageBox.Show("Az irányítószám kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(CityTextBox.Text))
            {
                MessageBox.Show("A város neve kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(CityRomanizedTextBox.Text))
            {
                MessageBox.Show("A város romanizált neve kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(StreetTextBox.Text))
            {
                MessageBox.Show("Az utca neve kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(StreetRomanizedTextBox.Text))
            {
                MessageBox.Show("Az utca romanizált neve kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (!int.TryParse(CapacityTextBox.Text, out int maxCapacity) || maxCapacity <= 0)
            {
                MessageBox.Show("A maximális kapacitás érvényes pozitív szám kell legyen!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (string.IsNullOrWhiteSpace(PhoneTextBox.Text))
            {
                MessageBox.Show("A telefonszám kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

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
                maxCapacity = maxCapacity,
                phoneNumber = PhoneTextBox.Text
            };

            var jsonContent = JsonSerializer.Serialize(location);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await HttpClientService.Client.PostAsync("/api/locations", content);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("A helyszín sikeresen hozzáadva!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);

                LocationAdded?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a helyszín hozzáadásakor: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}