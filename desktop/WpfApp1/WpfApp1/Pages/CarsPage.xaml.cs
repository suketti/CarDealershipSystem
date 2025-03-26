using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1
{
    public partial class CarsPage : Page
    {
        public CarsPage()
        {
            InitializeComponent();
            LoadCarsAsync();
        }

        private async void LoadCarsAsync()
        {
            try
            {
                var cars = await FetchCarsAsync();
                CarsListView.ItemsSource = cars;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching cars: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<List<CarDTO>> FetchCarsAsync()
        {
            HttpResponseMessage response = await HttpClientService.Client.GetAsync("/api/cars");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<CarDTO>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private void CarsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This method is triggered when the ListView selection is changed.
            // Add necessary code here if needed.
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            AddCarWindow addCarWindow = new AddCarWindow();
            addCarWindow.CarAdded += AddCarWindow_CarAdded;
            addCarWindow.ShowDialog();
        }

        private void AddCarWindow_CarAdded(object sender, EventArgs e)
        {
            LoadCarsAsync();
        }

        private void EditCar_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            if (editButton != null)
            {
                CarDTO selectedCar = editButton.DataContext as CarDTO;
                if (selectedCar != null)
                {
                    EditCarWindow editCarWindow = new EditCarWindow(selectedCar.ID);
                    editCarWindow.CarEdited += EditCarWindow_CarEdited;
                    editCarWindow.ShowDialog();
                }
            }
        }

        private void EditCarWindow_CarEdited(object sender, EventArgs e)
        {
            LoadCarsAsync();
        }

        private async void DeleteCar_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                CarDTO selectedCar = deleteButton.DataContext as CarDTO;
                if (selectedCar != null)
                {
                    var result = MessageBox.Show($"Are you sure you want to delete the car {selectedCar.CarModel.ModelNameEnglish}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            HttpResponseMessage response = await HttpClientService.Client.DeleteAsync($"/api/cars/{selectedCar.ID}");
                            response.EnsureSuccessStatusCode();
                            MessageBox.Show("Car deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadCarsAsync();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting car: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
    }
}