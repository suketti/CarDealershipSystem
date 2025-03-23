using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.Views
{
    public partial class EditEngineWindow : Window
    {
        private readonly HttpClient _httpClient;
        private readonly int _engineId;
        private EngineSizeModelDTO _engine;
        private List<FuelTypeDTO> _fuelTypes;

        public EditEngineWindow(int engineId, EngineSizeModelDTO engine)
        {
            InitializeComponent();
            _httpClient = HttpClientService.Client;
            _engineId = engineId;
            LoadFuelTypes();
            LoadEngineData(engine);
        }

        private async void LoadFuelTypes()
        {
            var response = await _httpClient.GetAsync("/api/cars/metadata/fuelTypes");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                _fuelTypes = JsonSerializer.Deserialize<List<FuelTypeDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                cbFuelTypes.ItemsSource = _fuelTypes;
            }
        }

        private void LoadEngineData(EngineSizeModelDTO engine)
        {
            _engine = engine;
            tbEngineSize.Text = _engine.EngineSize.ToString();
            cbFuelTypes.SelectedValue = _engine.FuelType.ID;
        }

        private async void btnSaveEngine_Click(object sender, RoutedEventArgs e)
        {
            var updatedEngine = new
            {
                id = _engineId,
                newEngineSize = int.Parse(tbEngineSize.Text),
                fuelTypeID = (int)cbFuelTypes.SelectedValue
            };

            var content = new StringContent(JsonSerializer.Serialize(updatedEngine), Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync("/api/cars/engine/update", content);
            if (response.IsSuccessStatusCode)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Hiba történt a motor frissítésekor.");
            }
        }
    }
}