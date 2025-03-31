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
                MessageBox.Show($"Hiba az autók lekérése közben: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
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
            // Ez a metódus akkor hívódik meg, amikor a ListView kiválasztása megváltozik.
            // Ha szükséges, itt adhatsz hozzá kódot.
        }

        private void AddCar_Click(object sender, RoutedEventArgs e)
        {
            AddCarWindow addCarWindow = new AddCarWindow();
            addCarWindow.CarAdded += AddCarWindow_CarAdded;
            addCarWindow.Closed += AddCarWindow_Closed; // Ensure refresh when closed
            addCarWindow.ShowDialog();
        }

        private void AddCarWindow_CarAdded(object sender, EventArgs e)
        {
            LoadCarsAsync();
        }

        private void AddCarWindow_Closed(object sender, EventArgs e)
        {
            LoadCarsAsync(); // Reload cars when the add window is closed
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
                    var result = MessageBox.Show($"Biztosan törölni szeretné a(z) {selectedCar.CarModel.ModelNameEnglish} autót?", "Törlés megerősítése", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            HttpResponseMessage response = await HttpClientService.Client.DeleteAsync($"/api/cars/{selectedCar.ID}");
                            response.EnsureSuccessStatusCode();
                            MessageBox.Show("Az autó sikeresen törölve!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadCarsAsync();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Hiba az autó törlése közben: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
    }
}
