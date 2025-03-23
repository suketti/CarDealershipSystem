using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1
{
    public partial class LocationsPage : Page
    {
        public LocationsPage()
        {
            InitializeComponent();
            LoadLocationsAsync();
        }

        private async void LoadLocationsAsync()
        {
            try
            {
                var locations = await FetchLocationsAsync();
                LocationsListView.ItemsSource = locations;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error fetching locations: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<List<LocationDTO>> FetchLocationsAsync()
        {
            HttpResponseMessage response = await HttpClientService.Client.GetAsync("/api/locations");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<LocationDTO>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private void AddLocation_Click(object sender, RoutedEventArgs e)
        {
            AddLocationWindow addLocationWindow = new AddLocationWindow();
            addLocationWindow.LocationAdded += AddLocationWindow_LocationAdded;
            addLocationWindow.ShowDialog();
        }

        private void AddLocationWindow_LocationAdded(object sender, EventArgs e)
        {
            LoadLocationsAsync();
        }

        private void LocationsListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // This method is triggered when the ListView selection is changed.
            // Add necessary code here if needed.
        }

        private void EditLocation_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            if (editButton != null)
            {
                LocationDTO selectedLocation = editButton.DataContext as LocationDTO;
                if (selectedLocation != null)
                {
                    EditLocationWindow editLocationWindow = new EditLocationWindow(selectedLocation);
                    editLocationWindow.LocationEdited += EditLocationWindow_LocationEdited;
                    editLocationWindow.ShowDialog();
                }
            }
        }

        private void EditLocationWindow_LocationEdited(object sender, EventArgs e)
        {
            LoadLocationsAsync();
        }

        private async void DeleteLocation_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                LocationDTO selectedLocation = deleteButton.DataContext as LocationDTO;
                if (selectedLocation != null)
                {
                    var result = MessageBox.Show($"Are you sure you want to delete the location {selectedLocation.LocationName}?", "Confirm Delete", MessageBoxButton.YesNo, MessageBoxImage.Warning);
                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            HttpResponseMessage response = await HttpClientService.Client.DeleteAsync($"/api/locations/{selectedLocation.Id}");
                            response.EnsureSuccessStatusCode();
                            MessageBox.Show("Location deleted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadLocationsAsync();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Error deleting location: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
        }
    }
}