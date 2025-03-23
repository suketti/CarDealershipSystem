using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using WpfApp1.Services;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class CarMakersPage : Page
    {
        private readonly HttpClient _httpClient;
        public ObservableCollection<CarMakerDTO> CarMakers { get; set; }

        public CarMakersPage()
        {
            InitializeComponent();
            _httpClient = HttpClientService.Client;
            CarMakers = new ObservableCollection<CarMakerDTO>();
            DataContext = this;
            LoadCarMakers();
        }

        private async void LoadCarMakers()
        {
            var makers = await FetchCarMakersAsync();
            if (makers != null)
            {
                foreach (var maker in makers.OrderBy(m => m.BrandEnglish))
                {
                    CarMakers.Add(maker);
                }
            }
            CarMakersDataGrid.ItemsSource = CarMakers;
        }

        private async Task<CarMakerDTO[]> FetchCarMakersAsync()
        {
            var response = await _httpClient.GetAsync("api/cars/makers");
            response.EnsureSuccessStatusCode();
            var responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<CarMakerDTO[]>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private async void CreateButton_Click(object sender, RoutedEventArgs e)
        {
            var brandEnglish = NewMakerBrandEnglishTextBox.Text;
            var brandJapanese = NewMakerBrandJapaneseTextBox.Text;
            if (string.IsNullOrWhiteSpace(brandEnglish) || string.IsNullOrWhiteSpace(brandJapanese))
            {
                MessageBox.Show("Mindkét mezőt ki kell tölteni.");
                return;
            }

            var newMaker = new CreateCarMakerDTO { BrandEnglish = brandEnglish, BrandJapanese = brandJapanese };
            var jsonContent = JsonSerializer.Serialize(newMaker);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await _httpClient.PostAsync("api/cars/makers", content);
                response.EnsureSuccessStatusCode();
                var responseBody = await response.Content.ReadAsStringAsync();
                var createdMaker = JsonSerializer.Deserialize<CarMakerDTO>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                if (createdMaker != null)
                {
                    CarMakers.Add(createdMaker);
                    CarMakers = new ObservableCollection<CarMakerDTO>(CarMakers.OrderBy(m => m.BrandEnglish));
                    CarMakersDataGrid.ItemsSource = CarMakers;
                }
                MessageBox.Show("Autógyártó sikeresen létrehozva!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt az autógyártó létrehozásakor: {ex.Message}");
            }
        }

        private async void EditButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var carMaker = button.DataContext as CarMakerDTO;
            if (carMaker == null) return;

            var editWindow = new EditCarMakerWindow(carMaker);
            if (editWindow.ShowDialog() == true)
            {
                var updatedMaker = editWindow.UpdatedCarMaker;
                var jsonContent = JsonSerializer.Serialize(updatedMaker);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                try
                {
                    var response = await _httpClient.PutAsync("api/cars/makers", content);
                    response.EnsureSuccessStatusCode();
                    MessageBox.Show("Autógyártó sikeresen szerkesztve!");

                    // Update the DataGrid with the edited car maker
                    var index = CarMakers.IndexOf(carMaker);
                    if (index >= 0)
                    {
                        CarMakers[index] = updatedMaker;
                        CarMakers = new ObservableCollection<CarMakerDTO>(CarMakers.OrderBy(m => m.BrandEnglish));
                        CarMakersDataGrid.ItemsSource = CarMakers;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt az autógyártó szerkesztésekor: {ex.Message}");
                }
            }
        }

        private async void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var carMaker = button.DataContext as CarMakerDTO;
            if (carMaker == null) return;

            var result = MessageBox.Show($"Biztosan törölni szeretné a(z) {carMaker.BrandEnglish} autógyártót?", "Törlés megerősítése", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                try
                {
                    var response = await _httpClient.DeleteAsync($"api/cars/makers/{carMaker.ID}");
                    response.EnsureSuccessStatusCode();
                    CarMakers.Remove(carMaker);
                    MessageBox.Show("Autógyártó sikeresen törölve!");
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Hiba történt az autógyártó törlésekor: {ex.Message}");
                }
            }
        }
    }


}