using System;
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
    public partial class CreateNewCarModelWindow : Window
    {
        private readonly HttpClient _httpClient;
        private readonly int? _modelId;
        private List<EngineSizeModelDTO> _engineSizes = new List<EngineSizeModelDTO>();
        private List<CarMakerDTO> _makers = new List<CarMakerDTO>();
        private List<FuelTypeDTO> _fuelTypes = new List<FuelTypeDTO>();

        public CreateNewCarModelWindow(int? modelId = null)
        {
            InitializeComponent();
            _httpClient = HttpClientService.Client;
            _modelId = modelId;
            LoadMakers();
            LoadFuelTypes();
            
            if (_modelId.HasValue)
            {
                LoadModel(_modelId.Value);
            }
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

        private async void LoadModel(int modelId)
        {
            var response = await _httpClient.GetAsync($"/api/cars/models/{modelId}");
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var model = JsonSerializer.Deserialize<CarModelDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
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
                dgEngines.ItemsSource = null;
                dgEngines.ItemsSource = _engineSizes;
            }
        }

        private async void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate inputs before proceeding
                if (cbMakers.SelectedValue == null)
                {
                    MessageBox.Show("Kérem válasszon egy gyártót.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbModelNameEnglish.Text) || string.IsNullOrWhiteSpace(tbModelNameJapanese.Text))
                {
                    MessageBox.Show("A modell neve nem lehet üres.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (string.IsNullOrWhiteSpace(tbModelCode.Text))
                {
                    MessageBox.Show("A modell kód megadása kötelező.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Attempt to parse each numeric field
                if (!int.TryParse(tbManufacturingStartYear.Text, out int manufacturingStartYear))
                {
                    MessageBox.Show("Kérem adja meg érvényes gyártási kezdő évet.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(tbManufacturingEndYear.Text, out int manufacturingEndYear))
                {
                    MessageBox.Show("Kérem adja meg érvényes gyártási befejező évet.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(tbPassengerCount.Text, out int passengerCount))
                {
                    MessageBox.Show("Kérem adja meg érvényes utasok számát.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(tbLength.Text, out int length))
                {
                    MessageBox.Show("Kérem adja meg érvényes hosszúságot.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(tbWidth.Text, out int width))
                {
                    MessageBox.Show("Kérem adja meg érvényes szélességet.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(tbHeight.Text, out int height))
                {
                    MessageBox.Show("Kérem adja meg érvényes magasságot.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                if (!int.TryParse(tbMass.Text, out int mass))
                {
                    MessageBox.Show("Kérem adja meg érvényes tömeget.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Prepare the model object
                var model = new CreateCarModelDTO
                {
                    MakerID = (int)cbMakers.SelectedValue,
                    ModelNameEnglish = tbModelNameEnglish.Text,
                    ModelNameJapanese = tbModelNameJapanese.Text,
                    ModelCode = tbModelCode.Text,
                    ManufacturingStartYear = manufacturingStartYear,
                    ManufacturingEndYear = manufacturingEndYear,
                    PassengerCount = passengerCount,
                    Length = length,
                    Width = width,
                    Height = height,
                    Mass = mass
                };

                HttpResponseMessage response;

                // Perform the appropriate API call (PUT if modelId exists, else POST)
                if (_modelId.HasValue)
                {
                    // Update existing car model
                    response = await _httpClient.PutAsync($"/api/cars/models/{_modelId}", new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));
                }
                else
                {
                    // Create new car model
                    response = await _httpClient.PostAsync("/api/cars/models", new StringContent(JsonSerializer.Serialize(model), Encoding.UTF8, "application/json"));
                }

                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    var createdModel = JsonSerializer.Deserialize<CarModelDTO>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                    // If creating a new model, add engines
                    if (createdModel != null)
                    {
                        await AddEnginesToModel(createdModel.ID);
                        this.DialogResult = true;
                        MessageBox.Show("Sikeresen hozzaadva!");
                        this.Close();
                    }
                    else
                    {
                        MessageBox.Show("Hiba történt a válasz feldolgozása során.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                else
                {
                    // Handle unsuccessful response
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Hiba történt: {errorMessage}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                // Catch any unexpected exceptions
                MessageBox.Show($"Hiba történt: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }



        private async Task AddEnginesToModel(int modelId)
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
                await _httpClient.PostAsync("/api/cars/engine", engineContent);
            }
        }
    }
}