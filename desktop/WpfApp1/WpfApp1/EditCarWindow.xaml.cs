using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Threading.Tasks;
using WpfApp1.Models;
using WpfApp1.Services;
using System.Collections.Generic;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class EditCarWindow : Window
    {
        private CarDTO car;
        public event EventHandler CarEdited;

        public EditCarWindow(CarDTO car)
        {
            InitializeComponent();
            this.car = car;
            LoadCarDetails();
            LoadComboBoxData();
        }

        private void LoadCarDetails()
        {
            BrandTextBox.Text = car.Brand.ID.ToString();
            ModelTextBox.Text = car.CarModel.ID.ToString();
            GradeTextBox.Text = car.Grade;
            BodyTypeComboBox.SelectedValue = car.BodyType.ID;
            LocationComboBox.SelectedValue = car.Location.Id;
            EngineSizeTextBox.Text = car.EngineSize.ID.ToString();
            FuelTypeComboBox.SelectedValue = car.FuelType.ID;
            DriveTrainComboBox.SelectedValue = car.DriveTrain.ID;
            TransmissionTypeComboBox.SelectedValue = car.TransmissionType.ID;
            ColorComboBox.SelectedValue = car.Color.ID;
            PriceTextBox.Text = car.Price;
            LicensePlateNumberTextBox.Text = car.LicensePlateNumber;
            MOTExpiryTextBox.Text = car.MOTExpiry?.ToString("yyyy-MM-dd");
            VINNumTextBox.Text = car.VINNum;
            RepairHistoryCheckBox.IsChecked = car.RepairHistory;
            IsInTransferCheckBox.IsChecked = car.IsInTransfer;
            IsSmokingCheckBox.IsChecked = car.IsSmoking;
        }

        private async void LoadComboBoxData()
        {
            // Load data for ComboBoxes (BodyType, Location, DriveTrain, TransmissionType, FuelType, Color)
            var bodyTypes = await FetchDataAsync<BodyTypeDTO>("/api/cars/metadata/bodytypes");
            BodyTypeComboBox.ItemsSource = bodyTypes;
            BodyTypeComboBox.DisplayMemberPath = "Name";
            BodyTypeComboBox.SelectedValuePath = "ID";

            var locations = await FetchDataAsync<LocationDTO>("/api/locations");
            LocationComboBox.ItemsSource = locations;
            LocationComboBox.DisplayMemberPath = "LocationName";
            LocationComboBox.SelectedValuePath = "Id";

            var driveTrains = await FetchDataAsync<DrivetrainTypeDTO>("/api/cars/metadata/drivetraintypes");
            DriveTrainComboBox.ItemsSource = driveTrains;
            DriveTrainComboBox.DisplayMemberPath = "Name";
            DriveTrainComboBox.SelectedValuePath = "ID";

            var transmissionTypes = await FetchDataAsync<TransmissionTypeDTO>("/api/cars/metadata/transmissiontypes");
            TransmissionTypeComboBox.ItemsSource = transmissionTypes;
            TransmissionTypeComboBox.DisplayMemberPath = "Name";
            TransmissionTypeComboBox.SelectedValuePath = "ID";

            var fuelTypes = await FetchDataAsync<FuelTypeDTO>("/api/cars/metadata/fueltypes");
            FuelTypeComboBox.ItemsSource = fuelTypes;
            FuelTypeComboBox.DisplayMemberPath = "NameEnglish";
            FuelTypeComboBox.SelectedValuePath = "ID";

            var colors = await FetchDataAsync<ColorDTO>("/api/cars/metadata/colors");
            ColorComboBox.ItemsSource = colors;
            ColorComboBox.DisplayMemberPath = "ColorNameEnglish";
            ColorComboBox.SelectedValuePath = "ID";
        }

        private async Task<List<T>> FetchDataAsync<T>(string endpoint)
        {
            HttpResponseMessage response = await HttpClientService.Client.GetAsync(endpoint);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<T>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private async void EditCar_Click(object sender, RoutedEventArgs e)
        {
            var updatedCar = new CreateCarDTO
            {
                Brand = int.Parse(BrandTextBox.Text),
                Model = int.Parse(ModelTextBox.Text),
                Grade = GradeTextBox.Text,
                BodyType = (int)BodyTypeComboBox.SelectedValue,
                Location = (int)LocationComboBox.SelectedValue,
                EngineSize = int.Parse(EngineSizeTextBox.Text),
                FuelType = (int)FuelTypeComboBox.SelectedValue,
                DriveTrain = (int)DriveTrainComboBox.SelectedValue,
                TransmissionType = (int)TransmissionTypeComboBox.SelectedValue,
                Color = (int)ColorComboBox.SelectedValue,
                Price = PriceTextBox.Text,
                LicensePlateNumber = LicensePlateNumberTextBox.Text,
                MOTExpiry = DateTime.Parse(MOTExpiryTextBox.Text),
                VINNum = VINNumTextBox.Text,
                RepairHistory = RepairHistoryCheckBox.IsChecked ?? false,
                IsInTransfer = IsInTransferCheckBox.IsChecked ?? false,
                IsSmoking = IsSmokingCheckBox.IsChecked ?? false
            };

            var jsonContent = JsonSerializer.Serialize(updatedCar);
            var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

            try
            {
                var response = await HttpClientService.Client.PutAsync($"/api/cars/{car.ID}", content);
                response.EnsureSuccessStatusCode();
                MessageBox.Show("Car edited successfully!");

                CarEdited?.Invoke(this, EventArgs.Empty);
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error editing car: {ex.Message}");
            }
        }
    }
}