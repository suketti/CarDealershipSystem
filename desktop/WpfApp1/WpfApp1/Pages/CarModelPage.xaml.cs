using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.Views
{
    public partial class CarModelPage : Page
    {
        private readonly HttpClient _httpClient;
        private ObservableCollection<CarModelDTO> _models;

        public CarModelPage()
        {
            InitializeComponent();
            _httpClient = HttpClientService.Client;
            LoadModels();
        }

        private async void LoadModels()
        {
            var response = await _httpClient.GetAsync("/api/cars/models");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                _models = JsonSerializer.Deserialize<ObservableCollection<CarModelDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                lvModelList.ItemsSource = _models;
            }
            else
            {
                MessageBox.Show("Hiba történt az adatok betöltésekor.");
            }
        }

        private void btnCreateModel_Click(object sender, RoutedEventArgs e)
        {
            var modelFormWindow = new CreateNewCarModelWindow();
            modelFormWindow.ShowDialog();
            LoadModels();
        }

        private void btnEditModel_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var modelId = (int)button.Tag;
            var selectedModel = _models.FirstOrDefault(m => m.ID == modelId);
            if (selectedModel != null)
            {
                var editModelWindow = new EditCarModelWindow(modelId, selectedModel);
                editModelWindow.ShowDialog();
                LoadModels();
            }
        }

        private async void btnDeleteModel_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var modelId = (int)button.Tag;
            var response = await _httpClient.DeleteAsync($"/api/cars/models/{modelId}");
            if (response.IsSuccessStatusCode)
            {
                _models.Remove(_models.First(m => m.ID == modelId));
            }
            else
            {
                MessageBox.Show("Hiba történt a modell törlésekor.");
            }
        }

        private void btnEditEngine_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var engineId = (int)button.Tag;
            var selectedModel = _models.FirstOrDefault(m => m.EngineSizes.Any(engine => engine.ID == engineId));
            var selectedEngine = selectedModel?.EngineSizes.FirstOrDefault(engine => engine.ID == engineId);
            if (selectedEngine != null)
            {
                var editEngineWindow = new EditEngineWindow(engineId, selectedEngine);
                editEngineWindow.ShowDialog();
                LoadModels();
            }
        }

        private async void btnDeleteEngine_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var engineId = (int)button.Tag;
            var response = await _httpClient.DeleteAsync($"/api/cars/engine/{engineId}");
            if (response.IsSuccessStatusCode)
            {
                LoadModels();
            }
            else
            {
                MessageBox.Show("Hiba történt a motor törlésekor.");
            }
        }
    }
}