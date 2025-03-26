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
using System.Linq;

namespace WpfApp1
{
    public partial class EditCarWindow : Window
    {
        public event EventHandler CarEdited;
        private List<string> SelectedImagePaths = new List<string>();
        private int CarId;

        public EditCarWindow(int carId)
        {
            InitializeComponent();
            CarId = carId;
            LoadCarData();
        }

        private async void LoadCarData()
        {
            try
            {
                // Fetch car details by ID
                var car = await FetchDataByIdAsync<CarDTO>($"/api/cars/{CarId}");
                if (car != null)
                {
                    await LoadComboBoxData(); // Ensure combo boxes are populated first
                    await LoadCarModelsForBrand(car.CarModel.Maker.ID);
                    
                    await PopulateCarDetails(car); // Now set the selected values
                    await LoadImages();
                }
                else
                {
                    MessageBox.Show("Error loading car data.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading car data: {ex.Message}");
            }
        }


        private async 

        Task
PopulateCarDetails(CarDTO car)
        {
            if (car == null) return;

            // Temporarily remove event handlers to prevent unwanted resets
            BrandComboBox.SelectionChanged -= BrandOrModelChanged;
            //ModelComboBox.SelectionChanged -= ModelComboBox_SelectionChanged;

            // Match and set the correct items
            BrandComboBox.SelectedItem = ((List<CarMakerDTO>)BrandComboBox.ItemsSource)?.FirstOrDefault(b => b.ID == car.Brand.ID);
            ModelComboBox.SelectedItem = ((List<CarModelDTO>)ModelComboBox.ItemsSource)?.FirstOrDefault(m => m.ID == car.CarModel.ID);
            await LoadDependentData();
            foreach (ComboBoxItem item in GradeComboBox.Items)
            {
                if (item.Content.ToString().Equals(car.Grade, StringComparison.OrdinalIgnoreCase))
                {
                    GradeComboBox.SelectedItem = item;
                    break;
                }
            }
            BodyTypeComboBox.SelectedItem = ((List<BodyTypeDTO>)BodyTypeComboBox.ItemsSource)?.FirstOrDefault(b => b.ID == car.BodyType.ID);
            LocationComboBox.SelectedItem = ((List<LocationDTO>)LocationComboBox.ItemsSource)?.FirstOrDefault(l => l.Id == car.Location.Id);
            EngineSizeComboBox.SelectedItem = ((List<EngineSizeModelDTO>)EngineSizeComboBox.ItemsSource)?.FirstOrDefault(e => e.ID == car.EngineSize.ID);
            FuelTypeComboBox.SelectedItem = ((List<FuelTypeDTO>)FuelTypeComboBox.ItemsSource)?.FirstOrDefault(f => f.ID == car.FuelType.ID);
            DriveTrainComboBox.SelectedItem = ((List<DrivetrainTypeDTO>)DriveTrainComboBox.ItemsSource)?.FirstOrDefault(d => d.ID == car.DriveTrain.ID);
            TransmissionTypeComboBox.SelectedItem = ((List<TransmissionTypeDTO>)TransmissionTypeComboBox.ItemsSource)?.FirstOrDefault(t => t.ID == car.TransmissionType.ID);
            ColorComboBox.SelectedItem = ((List<ColorDTO>)ColorComboBox.ItemsSource)?.FirstOrDefault(c => c.ID == car.Color.ID);

            // Set text fields
            PriceTextBox.Text = car.Price.ToString();
            LicensePlateNumberTextBox.Text = car.LicensePlateNumber;
            MOTExpiryTextBox.Text = car.MOTExpiry?.ToString("yyyy-MM-dd");
            VINNumTextBox.Text = car.VINNum;

            // Set checkboxes
            RepairHistoryCheckBox.IsChecked = car.RepairHistory;
            IsInTransferCheckBox.IsChecked = car.IsInTransfer;
            IsSmokingCheckBox.IsChecked = car.IsSmoking;

            // Set mileage text
            MileageTextBox.Text = car.Mileage.ToString();

            // Reattach event handlers
            BrandComboBox.SelectionChanged += BrandOrModelChanged;
            //ModelComboBox.SelectionChanged += ModelComboBox_SelectionChanged;
        }


        private async void BrandOrModelChanged(object sender, SelectionChangedEventArgs e)
        {
            // Reset dependent ComboBoxes and disable them
            ResetComboBoxes();

            if (BrandComboBox.SelectedValue != null)
            {
                int brandId = (int)BrandComboBox.SelectedValue;
                await LoadCarModelsForBrand(brandId);
            }
            else
            {
                ModelComboBox.ItemsSource = null; // Clear items in the ModelComboBox
                ModelComboBox.IsEnabled = false; // Disable model ComboBox if no brand is selected
                EnableComboBoxes(false); // Disable all dependent fields
            }
        }

        private async Task LoadCarModelsForBrand(int brandId)
        {
            try
            {
                // Fetch the models based on the selected brand ID
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
            FuelTypeComboBox.ItemsSource = null;
            FuelTypeComboBox.IsEnabled = false;
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
            FuelTypeComboBox.IsEnabled = enable;
            DriveTrainComboBox.IsEnabled = enable;
            TransmissionTypeComboBox.IsEnabled = enable;
            ColorComboBox.IsEnabled = enable;
            PriceTextBox.IsEnabled = enable;
            LicensePlateNumberTextBox.IsEnabled = enable;
            MOTExpiryTextBox.IsEnabled = enable;
            VINNumTextBox.IsEnabled = enable;
            RepairHistoryCheckBox.IsEnabled = enable;
            IsInTransferCheckBox.IsEnabled = enable;
            IsSmokingCheckBox.IsEnabled = enable;
            EditCarButton.IsEnabled = enable;
            MileageTextBox.IsEnabled = enable;
        }

        private async Task LoadComboBoxData()
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

        private List<BodyTypeDTO> bodyTypes;
        private List<LocationDTO> locations;
        private List<DrivetrainTypeDTO> driveTrains;
        private List<TransmissionTypeDTO> transmissionTypes;
        private List<FuelTypeDTO> fuelTypes;
        private List<ColorDTO> colors;
        private List<CarModelDTO> models;

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

        private async Task<T> FetchDataByIdAsync<T>(string endpoint)
        {
            try
            {
                var response = await HttpClientService.Client.GetAsync(endpoint);

                // Handle successful response
                if (response.IsSuccessStatusCode)
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    return JsonSerializer.Deserialize<T>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
                }
                else
                {
                    // Handle error codes (e.g., 500, 400)
                    MessageBox.Show($"Error: {response.StatusCode.ToString()}");
                    return default(T); // Return default value in case of errors
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error while fetching data: {ex.Message}");
                return default(T); // Return default value in case of an exception
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


        private async void ModelComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BrandComboBox.SelectedValue != null && ModelComboBox.SelectedValue != null)
            {
                await LoadDependentData();
            }
            else
            {
                EnableComboBoxes(true); 
            }
        }

        private async Task LoadDependentData()
        {
            try
            {
                int modelId = (int)ModelComboBox.SelectedValue;
                var engineSizes = await FetchDataAsync<EngineSizeModelDTO>($"/api/cars/engine/model/{modelId}");
                EngineSizeComboBox.ItemsSource = engineSizes;
                EngineSizeComboBox.DisplayMemberPath = "EngineSize";
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

                FuelTypeComboBox.ItemsSource = fuelTypes;
                FuelTypeComboBox.DisplayMemberPath = "NameEnglish";
                FuelTypeComboBox.SelectedValuePath = "ID";

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

        private async void EditCar_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var updatedCar = new CreateCarDTO
                {
                    Brand = (int)BrandComboBox.SelectedValue,
                    Model = (int)ModelComboBox.SelectedValue,
                    Grade = (GradeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    BodyType = (int)BodyTypeComboBox.SelectedValue,
                    Location = (int)LocationComboBox.SelectedValue,
                    EngineSize = (int)EngineSizeComboBox.SelectedValue,
                    FuelType = (int)FuelTypeComboBox.SelectedValue,
                    DriveTrain = (int)DriveTrainComboBox.SelectedValue,
                    TransmissionType = (int)TransmissionTypeComboBox.SelectedValue,
                    Color = (int)ColorComboBox.SelectedValue,
                    Price = PriceTextBox.Text,
                    LicensePlateNumber = LicensePlateNumberTextBox.Text,
                    MOTExpiry = DateTime.TryParse(MOTExpiryTextBox.Text, out DateTime motExpiry) ? motExpiry : (DateTime?)null,
                    VINNum = VINNumTextBox.Text,
                    RepairHistory = RepairHistoryCheckBox.IsChecked ?? false,
                    IsInTransfer = IsInTransferCheckBox.IsChecked ?? false,
                    IsSmoking = IsSmokingCheckBox.IsChecked ?? false,
                    Extras = new List<int> { 0 },
                    Mileage = int.TryParse(MileageTextBox.Text, out int mileage) ? mileage : 0
                };

                var jsonContent = JsonSerializer.Serialize(updatedCar);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                var response = await HttpClientService.Client.PutAsync($"/api/cars/{CarId}", content);

                if (response.StatusCode == System.Net.HttpStatusCode.BadRequest)
                {
                    MessageBox.Show("Hibás mezők találhatók. Kérlek, ellenőrizd a bevitt adatokat!");
                    return;
                }

                response.EnsureSuccessStatusCode();

                await UploadImagesForCar(CarId);

                MessageBox.Show("Az autó sikeresen frissítve!");
                CarEdited?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error updating car: {ex.Message}");
            }
        }

        private async Task LoadImages()
        {
            try
            {
                var imageUrls = await FetchDataAsync<string>($"/api/images/car/{CarId}");
                if (imageUrls != null)
                {
                    foreach (var url in imageUrls)
                    {
                        var localPath = await DownloadImage(url);
                        if (!string.IsNullOrEmpty(localPath))
                        {
                            SelectedImagePaths.Add(localPath);
                        }
                    }
                    ImageList.ItemsSource = new List<string>(SelectedImagePaths);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading images: {ex.Message}");
            }
        }

        private async Task<string> DownloadImage(string url)
        {
            try
            {
                var response = await HttpClientService.Client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);
                response.EnsureSuccessStatusCode();

                var fileName = Path.GetFileName(url);
                var localPath = Path.Combine(Path.GetTempPath(), fileName);

                using (var fs = new FileStream(localPath, FileMode.Create, FileAccess.Write, FileShare.None))
                {
                    await response.Content.CopyToAsync(fs);
                }

                return localPath;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error downloading image: {ex.Message}");
                return null;
            }
        }

        private async void BrowseImages_Click(object sender, RoutedEventArgs e)
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
                // Step 1: Get already uploaded images from the server
                var existingImagesResponse = await HttpClientService.Client.GetAsync($"/api/images/car/{carId}");
                existingImagesResponse.EnsureSuccessStatusCode();
                var existingImagesJson = await existingImagesResponse.Content.ReadAsStringAsync();
                var existingImages = JsonSerializer.Deserialize<List<string>>(existingImagesJson) ?? new List<string>();

                // Step 2: Filter images that haven't been uploaded yet
                var imagesToUpload = SelectedImagePaths
                    .Where(filePath => !string.IsNullOrEmpty(filePath) && File.Exists(filePath))
                    .Where(filePath => !existingImages.Any(img => img.Contains(Path.GetFileName(filePath)))) // Check by filename
                    .ToList();

                if (!imagesToUpload.Any())
                { 
                    return;
                }

                // Step 3: Upload only the new images
                foreach (var filePath in imagesToUpload)
                {
                    byte[] imageData = File.ReadAllBytes(filePath);
                    var imageContent = new MultipartFormDataContent
            {
                { new StringContent(carId.ToString()), "CarID" },
                { new ByteArrayContent(imageData), "ImageFile", Path.GetFileName(filePath) }
            };

                    var imageResponse = await HttpClientService.Client.PostAsync("/api/images/upload", imageContent);
                    imageResponse.EnsureSuccessStatusCode();
                }

                MessageBox.Show("Az uj kepek sikeresen feltoltve");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Nem sikerult a kepek feltoltese!: {ex.Message}");
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