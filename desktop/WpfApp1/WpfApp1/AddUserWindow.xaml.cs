using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Net.Http;
using System.Text.Json;
using WpfApp1.Services;
using WpfApp1.Models;
using System.Text;
using System.Windows.Controls;

namespace WpfApp1
{
    public partial class AddUserWindow : Window
    {
        // Declare the UserAdded event
        public event EventHandler UserAdded;

        public AddUserWindow()
        {
            InitializeComponent();
        }

        // Fetch Locations based on selected role (only for Dealers)
        private async void FetchLocations()
        {
            try
            {
                // Fetch locations asynchronously using the new method
                List<LocationDTO> locations = await FetchLocationsAsync();

                // Check if locations were fetched successfully
                if (locations != null && locations.Count > 0)
                {
                    LocationComboBox.ItemsSource = locations;
                    LocationComboBox.IsEnabled = true;
                }
                else
                {
                    MessageBox.Show("Nincsenek elérhető helyek.");
                    LocationComboBox.IsEnabled = false;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a helyek lekérésekor: {ex.Message}");
            }
        }

        private async Task<List<LocationDTO>> FetchLocationsAsync()
        {
            HttpResponseMessage response = await HttpClientService.Client.GetAsync("/api/locations");
            response.EnsureSuccessStatusCode(); // Ensure the request was successful

            // Read the response body as a string
            string responseBody = await response.Content.ReadAsStringAsync();

            // Deserialize the JSON response into a list of LocationDTO objects
            return JsonSerializer.Deserialize<List<LocationDTO>>(responseBody, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true // Handle case insensitivity in JSON keys
            });
        }

        private void RoleComboBox_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var selectedRole = (RoleComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();

            // If the selected role is Dealer, fetch locations; otherwise, disable LocationComboBox
            if (selectedRole == "Dealer")
            {
                FetchLocations();
            }
            else
            {
                LocationComboBox.IsEnabled = false;
            }
        }

        // Handle Add User button click
        // Handle Add User button click
        private async void AddUser_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string nameKanji = NameKanjiTextBox.Text;
            string email = EmailTextBox.Text;
            string phoneNumber = PhoneNumberTextBox.Text;
            string preferredLanguage = (PreferredLanguageComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            string role = (RoleComboBox.SelectedItem as System.Windows.Controls.ComboBoxItem)?.Content.ToString();
            LocationDTO selectedLocation = LocationComboBox.SelectedItem as LocationDTO;
            string password = PasswordBox.Password;
            string confirmPassword = ConfirmPasswordBox.Password;

            // Basic validation
            if (string.IsNullOrWhiteSpace(name))
            {
                MessageBox.Show("A név megadása kötelező.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(email))
            {
                MessageBox.Show("Az email cím megadása kötelező.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
            {
                MessageBox.Show("A jelszó és a jelszó megerősítése nem lehet üres.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (password != confirmPassword)
            {
                MessageBox.Show("A jelszavak nem egyeznek.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Validate phone number format
            if (string.IsNullOrWhiteSpace(phoneNumber) || !IsPhoneNumberValid(phoneNumber))
            {
                MessageBox.Show("Kérem adja meg érvényes telefonszámot.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ensure that a role is selected
            if (string.IsNullOrWhiteSpace(role))
            {
                MessageBox.Show("Kérem válasszon szerepkört.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Ensure that a preferred language is selected
            if (string.IsNullOrWhiteSpace(preferredLanguage))
            {
                MessageBox.Show("Kérem válasszon preferált nyelvet.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // If the role is Dealer, ensure Location is selected
            if (role == "Dealer" && selectedLocation == null)
            {
                MessageBox.Show("Kérem válasszon helyszínt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Prepare the DTO to send to the server
            var userDto = new AdminUserCreateDTO
            {
                Name = name,
                NameKanji = nameKanji,
                Email = email,
                PhoneNumber = phoneNumber,
                PreferredLanguage = preferredLanguage,
                Role = role,
                Password = password
            };

            // If the role is "Dealer", include the selected location
            if (role == "Dealer" && selectedLocation != null)
            {
                userDto.Location = selectedLocation;
            }

            try
            {
                // Serialize the userDto to JSON using the correct camelCase policy
                var json = JsonSerializer.Serialize(userDto, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                // Post the request to the server
                HttpResponseMessage response = await HttpClientService.Client.PostAsync("/api/users", content);

                if (response.IsSuccessStatusCode)
                {
                    MessageBox.Show("Felhasználó sikeresen hozzáadva!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                    // Trigger UserAdded event after successful creation
                    UserAdded?.Invoke(this, EventArgs.Empty);
                    this.Close(); // Close the window after success
                }
                else
                {
                    string errorMessage = await response.Content.ReadAsStringAsync();
                    MessageBox.Show($"Hiba történt: {errorMessage}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hálózati hiba: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }


        // Handle Cancel button click
        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        // Phone number validation (allow digits and + sign)
        private bool IsPhoneNumberValid(string phoneNumber)
        {
            foreach (char c in phoneNumber)
            {
                if (!char.IsDigit(c) && c != '+')
                {
                    return false;
                }
            }
            return true;
        }

        private void PhoneNumberTextBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            // Check if the input is either a digit or the '+' symbol
            if (!char.IsDigit(e.Text, 0) && e.Text != "+")
            {
                e.Handled = true;
            }
        }
    }
}
