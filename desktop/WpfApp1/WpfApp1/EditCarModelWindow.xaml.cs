using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1.Views
{
    public partial class EditCarModelWindow : Window
    {
        private readonly HttpClient _httpClient;
        private readonly int _modelId;
        private List<EngineSizeModelDTO> _engineSizes = new List<EngineSizeModelDTO>();
        private List<CarMakerDTO> _makers = new List<CarMakerDTO>();
        private List<FuelTypeDTO> _fuelTypes = new List<FuelTypeDTO>();

        public EditCarModelWindow(int modelId, CarModelDTO model)
        {
            InitializeComponent();
            _httpClient = HttpClientService.Client;
            _modelId = modelId;
            LoadMakers();
            LoadFuelTypes();
            LoadModelData(model);
        }

        private async void LoadMakers()
        {
            var response = await _httpClient.GetAsync("/api/cars/makers");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                _makers = JsonSerializer.Deserialize<List<CarMakerDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                cbMakers.ItemsSource = _makers;
            }
        }

        private async void LoadFuelTypes()
        {
            var response = await _httpClient.GetAsync("/api/cars/metadata/fuelTypes");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                _fuelTypes = JsonSerializer.Deserialize<List<FuelTypeDTO>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
        }

        private void LoadModelData(CarModelDTO model)
        {
            cbMakers.SelectedValue = model.Maker.ID;
            tbModelNameEnglish.Text = model.ModelNameEnglish;
            tbModelNameJapanese.Text = model.ModelNameJapanese;
            tbModelCode.Text = model.ModelCode;
            tbManufacturingStartYear.Text = model.ManufacturingStartYear.ToString();
            tbManufacturingEndYear.Text = model.ManufacturingEndYear.ToString();
            tbPassengerCount.Text = model.PassengerCount.ToString();
            tbLength.Text = model.Length.ToString();
            tbWidth.Text = model.Width.ToString();
            tbHeight.Text = model.Height.ToString();
            tbMass.Text = model.Mass.ToString();
            _engineSizes = model.EngineSizes;
            cbEngines.ItemsSource = _engineSizes;
        }

        private void btnAddEngine_Click(object sender, RoutedEventArgs e)
        {
            var addEngineWindow = new AddEngineWindow(_fuelTypes);
            if (addEngineWindow.ShowDialog() == true)
            {
                var newEngine = addEngineWindow.NewEngine;
                _engineSizes.Add(newEngine);
                cbEngines.ItemsSource = null;
                cbEngines.ItemsSource = _engineSizes;
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            var model = new CreateCarModelDTO
            {
                MakerID = (int)cbMakers.SelectedValue,
                ModelNameEnglish = tbModelNameEnglish.Text,
                ModelNameJapanese = tbModelNameJapanese.Text,
                ModelCode = tbModelCode.Text,
                ManufacturingStartYear = int.Parse(tbManufacturingStartYear.Text),
                ManufacturingEndYear = int.Parse(tbManufacturingEndYear.Text),
                PassengerCount = int.Parse(tbPassengerCount.Text),
                Length = int.Parse(tbLength.Text),
                Width = int.Parse(tbWidth.Text),
                Height = int.Parse(tbHeight.Text),
                Mass = int.Parse(tbMass.Text)
            };

            var response = await _httpClient.PutAsync($"/api/cars/models/{_modelId}", new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));
            if (response.IsSuccessStatusCode)
            {
                await UpdateEngines(_modelId);
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                MessageBox.Show("Hiba történt a modell frissítésekor.");
            }
        }

        private async Task UpdateEngines(int modelId)
        {
            foreach (var engine in _engineSizes)
            {
                var engineData = new
                {
                    modelID = modelId,
                    fuelType = engine.FuelType.ID,
                    engineSize = engine.EngineSize
                };

                var engineContent = new StringContent(JsonSerializer.Serialize(engineData), Encoding.UTF8, "application/json");
                await _httpClient.PutAsync("/api/cars/engine/update", engineContent);
            }
        }
    }
}