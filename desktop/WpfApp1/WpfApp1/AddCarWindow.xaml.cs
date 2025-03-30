using System;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Windows;
using System.Windows.Controls;
using System.Threading.Tasks;
using System.Collections.Generic;
using WpfApp1.Models;
using WpfApp1.Services;
using System.IO;
using Microsoft.Win32;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Windows.Input;

namespace WpfApp1
{
    public partial class AddCarWindow : Window
    {
        public event EventHandler CarAdded;
        private List<string> SelectedImagePaths = new List<string>();
        private List<BodyTypeDTO> bodyTypes;
        private List<LocationDTO> locations;
        private List<DrivetrainTypeDTO> driveTrains;
        private List<TransmissionTypeDTO> transmissionTypes;
        private List<FuelTypeDTO> fuelTypes;
        private List<ColorDTO> colors;
        private List<CarModelDTO> models;

        public AddCarWindow()
        {
            InitializeComponent();
            LoadComboBoxData();
        }

        private async void BrandOrModelChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reset dependent ComboBoxes and disable them
            ResetComboBoxes();

            if (BrandComboBox.SelectedValue != null)
            {
                try
                {
                    // Fetch the models based on the selected brand ID
                    int brandId = (int)BrandComboBox.SelectedValue;
                    var models = await FetchDataAsync<CarModelDTO>($"/api/cars/models/maker/{brandId}");

                    // Check if we have models and populate the ModelComboBox
                    if (models != null && models.Count > 0)
                    {
                        ModelComboBox.ItemsSource = models;
                        ModelComboBox.DisplayMemberPath = "ModelNameEnglish";
                        ModelComboBox.SelectedValuePath = "ID";
                        ModelComboBox.IsEnabled = true; // Enable model ComboBox
                    }
                    else
                    {
                        // No models for the selected brand, disable ModelComboBox and other fields
                        ModelComboBox.ItemsSource = null;
                        ModelComboBox.IsEnabled = false;
                        EnableComboBoxes(false); // Disable other fields as well
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading models: {ex.Message}");
                }
            }
            else
            {
                ModelComboBox.ItemsSource = null; // Clear items in the ModelComboBox
                ModelComboBox.IsEnabled = false; // Disable model ComboBox if no brand is selected
                EnableComboBoxes(false); // Disable all dependent fields
            }
        }


        private void ResetComboBoxes()
        {
            // Reset ModelComboBox
            ModelComboBox.ItemsSource = null;
            ModelComboBox.IsEnabled = false; // Disable it initially until a brand is selected

            // Reset other ComboBoxes if needed
            EngineSizeComboBox.ItemsSource = null;
            EngineSizeComboBox.IsEnabled = false;
            BodyTypeComboBox.ItemsSource = null;
            BodyTypeComboBox.IsEnabled = false;
            LocationComboBox.ItemsSource = null;
            LocationComboBox.IsEnabled = false;
            DriveTrainComboBox.ItemsSource = null;
            DriveTrainComboBox.IsEnabled = false;
            TransmissionTypeComboBox.ItemsSource = null;
            TransmissionTypeComboBox.IsEnabled = false;
            ColorComboBox.ItemsSource = null;
            ColorComboBox.IsEnabled = false;
        }

        private void EnableComboBoxes(bool enable)
        {
            // Enable/Disable other combo boxes depending on the availability of data
            GradeComboBox.IsEnabled = enable;
            BodyTypeComboBox.IsEnabled = enable;
            LocationComboBox.IsEnabled = enable;
            EngineSizeComboBox.IsEnabled = enable;
            DriveTrainComboBox.IsEnabled = enable;
            TransmissionTypeComboBox.IsEnabled = enable;
            ColorComboBox.IsEnabled = enable;
            PriceTextBox.IsEnabled = enable;
            LicensePlateNumberTextBox.IsEnabled = enable;
            MOTExpiryDatePicker.IsEnabled = enable;
            VINNumTextBox.IsEnabled = enable;
            RepairHistoryCheckBox.IsEnabled = enable;
            IsInTransferCheckBox.IsEnabled = enable;
            IsSmokingCheckBox.IsEnabled = enable;
            AddCarButton.IsEnabled = enable;
            MileageTextBox.IsEnabled = enable;
        }



        private async void LoadComboBoxData()
        {
            try
            {
                // Load Brand first
                var brands = await FetchDataAsync<CarMakerDTO>("/api/cars/makers");
                BrandComboBox.ItemsSource = brands;
                BrandComboBox.DisplayMemberPath = "BrandEnglish";
                BrandComboBox.SelectedValuePath = "ID";

                // Other ComboBoxes disabled until both Brand and Model are selected
                EnableComboBoxes(false);

                // Preload metadata for other ComboBoxes (used after enabling)
                await LoadMetadata();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}");
            }
        }



        private async Task<List<T>> FetchDataAsync<T>(string endpoint)
        {
            try
            {
                var response = await HttpClientService.Client.GetAsync(endpoint);

                // Handle successful response
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<List<T>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                // Handle specific 404 case for no models found (valid)
                else if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                {
                    // Check if the 404 is expected (no models for the brand)
                    if (endpoint.Contains("models/maker"))
                    {
                        return new List<T>(); // Return an empty list for valid 404 (no models)
                    }
                    else
                    {
                        // Other 404s are considered errors
                        MessageBox.Show("Error: The requested resource was not found.");
                        return new List<T>(); // Return an empty list for other errors
                    }
                }
                else
                {
                    // Handle other error codes (e.g., 500, 400)
                    MessageBox.Show($"Error: {response.StatusCode.ToString()}");
                    return new List<T>(); // Return an empty list in case of other errors
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while fetching data: {ex.Message}");
                return new List<T>(); // Return an empty list in case of an exception
            }
        }


        private async Task LoadMetadata()
        {
            bodyTypes = await FetchDataAsync<BodyTypeDTO>("/api/cars/metadata/bodytypes");
            locations = await FetchDataAsync<LocationDTO>("/api/locations");
            driveTrains = await FetchDataAsync<DrivetrainTypeDTO>("/api/cars/metadata/drivetrainTypes");
            transmissionTypes = await FetchDataAsync<TransmissionTypeDTO>("/api/cars/metadata/transmissiontypes");
            fuelTypes = await FetchDataAsync<FuelTypeDTO>("/api/cars/metadata/fuelTypes");
            colors = await FetchDataAsync<ColorDTO>("/api/cars/metadata/colors");
        }

        

        private async Task LoadModelsForSelectedBrand()
        {
            try
            {
                int brandId = (int)BrandComboBox.SelectedValue;
                models = await FetchDataAsync<CarModelDTO>($"/api/cars/models/{brandId}");
                ModelComboBox.ItemsSource = models;
                ModelComboBox.DisplayMemberPath = "ModelNameEnglish";
                ModelComboBox.SelectedValuePath = "ID";

                // Enable controls only when both Brand and Model are selected
                if (ModelComboBox.SelectedValue != null)
                {
                    await LoadDependentData();
                }
                else
                {
                    EnableComboBoxes(false);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading models: {ex.Message}");
            }
        }

        private async void ModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BrandComboBox.SelectedValue != null && ModelComboBox.SelectedValue != null)
            {
                await LoadDependentData();
            }
            else
            {
                EnableComboBoxes(false); // Disable dependent fields if model isn't selected
            }
        }


        private async Task LoadDependentData()
        {
            try
            {
                int modelId = (int)ModelComboBox.SelectedValue;
                var engineSizes = await FetchDataAsync<EngineSizeModelDTO>($"/api/cars/engine/model/{modelId}");
                EngineSizeComboBox.ItemsSource = engineSizes;
                EngineSizeComboBox.DisplayMemberPath = "DisplayText";
                EngineSizeComboBox.SelectedValuePath = "ID";

                // Populate other ComboBoxes
                BodyTypeComboBox.ItemsSource = bodyTypes;
                BodyTypeComboBox.DisplayMemberPath = "NameEnglish";
                BodyTypeComboBox.SelectedValuePath = "ID";

                LocationComboBox.ItemsSource = locations;
                LocationComboBox.DisplayMemberPath = "LocationName";
                LocationComboBox.SelectedValuePath = "Id";

                DriveTrainComboBox.ItemsSource = driveTrains;
                DriveTrainComboBox.DisplayMemberPath = "Type";
                DriveTrainComboBox.SelectedValuePath = "ID";

                TransmissionTypeComboBox.ItemsSource = transmissionTypes;
                TransmissionTypeComboBox.DisplayMemberPath = "Type";
                TransmissionTypeComboBox.SelectedValuePath = "ID";

                ColorComboBox.ItemsSource = colors;
                ColorComboBox.DisplayMemberPath = "ColorNameEnglish";
                ColorComboBox.SelectedValuePath = "ID";

                // Enable controls now
                EnableComboBoxes(true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading dependent data: {ex.Message}");
            }
        }


        private async void AddCar_Click(object sender, RoutedEventArgs e)
        {
            // Validate mandatory fields
            if (string.IsNullOrEmpty(BrandComboBox.Text))
            {
                MessageBox.Show("A márka mező kitöltése kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(ModelComboBox.Text))
            {
                MessageBox.Show("A modell mező kitöltése kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(GradeComboBox.Text))
            {
                MessageBox.Show("Az osztályzat mező kitöltése kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(BodyTypeComboBox.Text))
            {
                MessageBox.Show("A karosszéria típus mező kitöltése kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(LocationComboBox.Text))
            {
                MessageBox.Show("A helyszín mező kitöltése kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(EngineSizeComboBox.Text))
            {
                MessageBox.Show("A motor méret mező kitöltése kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(DriveTrainComboBox.Text))
            {
                MessageBox.Show("A meghajtás mező kitöltése kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(TransmissionTypeComboBox.Text))
            {
                MessageBox.Show("A sebességváltó típus mező kitöltése kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(ColorComboBox.Text))
            {
                MessageBox.Show("A szín mező kitöltése kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (string.IsNullOrEmpty(PriceTextBox.Text))
            {
                MessageBox.Show("Az ár mező kitöltése kötelező!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Validate optional fields
            if (string.IsNullOrEmpty(LicensePlateNumberTextBox.Text))
            {
                LicensePlateNumberTextBox.Text = null;  // Optional, if left empty
            }

            DateTime? MOTExpiry = MOTExpiryDatePicker.SelectedDate;
            if (MOTExpiry == null)
            {
                MOTExpiry = null;  // Optional, if left empty
            }

            if (string.IsNullOrEmpty(VINNumTextBox.Text))
            {
                VINNumTextBox.Text = null;  // Optional, if left empty
            }

            if (string.IsNullOrEmpty(MileageTextBox.Text))
            {
                MileageTextBox.Text = null;  // Optional, if left empty
            }

            // Optional checkboxes
            bool RepairHistory = RepairHistoryCheckBox.IsChecked ?? false;
            bool IsInTransfer = IsInTransferCheckBox.IsChecked ?? false;
            bool IsSmoking = IsSmokingCheckBox.IsChecked ?? false;


            try
            {
                var newCar = new CreateCarDTO
                {
                    Brand = (int)BrandComboBox.SelectedValue,
                    Model = (int)ModelComboBox.SelectedValue,
                    Grade = (GradeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    BodyType = (int)BodyTypeComboBox.SelectedValue,
                    Location = (int)LocationComboBox.SelectedValue,
                    EngineSize = (int)EngineSizeComboBox.SelectedValue,
                    FuelType = (int)(EngineSizeComboBox.SelectedItem as EngineSizeModelDTO).FuelType.ID,
                    DriveTrain = (int)DriveTrainComboBox.SelectedValue,
                    TransmissionType = (int)TransmissionTypeComboBox.SelectedValue,
                    Color = (int)ColorComboBox.SelectedValue,
                    Price = PriceTextBox.Text,
                    LicensePlateNumber = LicensePlateNumberTextBox.Text,
                    MOTExpiry = MOTExpiryDatePicker.SelectedDate ?? null,
                    VINNum = VINNumTextBox.Text,
                    RepairHistory = RepairHistoryCheckBox.IsChecked ?? false,
                    IsInTransfer = IsInTransferCheckBox.IsChecked ?? false,
                    IsSmoking = IsSmokingCheckBox.IsChecked ?? false,
                    Extras = new List<int> { 0 },
                    Mileage = int.TryParse(MileageTextBox.Text, out int mileage) ? mileage : 0
                };


                var jsonContent = JsonSerializer.Serialize(newCar);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await HttpClientService.Client.PostAsync("/api/cars", content);
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var carResponseData = JsonSerializer.Deserialize<CarDTO>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                int carId = carResponseData.ID;

                await UploadImagesForCar(carId);

                MessageBox.Show("Car added successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error adding car: {ex.Message}");
            }
        }

        private void BrowseImages_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif",
                Multiselect = true,
                Title = "Select Images"
            };

            if (openFileDialog.ShowDialog() == true)
            {
                SelectedImagePaths.AddRange(openFileDialog.FileNames);
                ImageList.ItemsSource = new List<string>(SelectedImagePaths);
            }
        }

        private void RemoveImage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string filePath)
            {
                SelectedImagePaths.Remove(filePath);
                ImageList.ItemsSource = new List<string>(SelectedImagePaths);
            }
        }

        private void OpenImage_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Tag is string filePath && File.Exists(filePath))
            {
                Process.Start(new ProcessStartInfo(filePath) { UseShellExecute = true });
            }
        }

        private async Task UploadImagesForCar(int carId)
        {
            try
            {
                foreach (var filePath in SelectedImagePaths)
                {
                    if (string.IsNullOrEmpty(filePath) || !File.Exists(filePath))
                        continue;

                    byte[] imageData = File.ReadAllBytes(filePath);
                    var imageContent = new MultipartFormDataContent
            {
                { new StringContent(carId.ToString()), "CarID" },
                { new ByteArrayContent(imageData), "ImageFile", Path.GetFileName(filePath) }
            };

                    var imageResponse = await HttpClientService.Client.PostAsync("/api/images/upload", imageContent);
                    imageResponse.EnsureSuccessStatusCode();
                }

                MessageBox.Show("Images uploaded successfully!");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error uploading images: {ex.Message}");
            }
        }
        private void MileageTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !Regex.IsMatch(e.Text, "^[0-9]+$"); // Only allows digits
        }

        private void MileageTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(MileageTextBox.Text))
            {
                MileageTextBox.Tag = null; // Treat empty as null
            }
            else if (int.TryParse(MileageTextBox.Text, out int mileage))
            {
                MileageTextBox.Tag = mileage; // Store parsed value in Tag
            }
        }

    }
}
