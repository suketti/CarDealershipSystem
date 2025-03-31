using System;
using System.Collections.Generic;
using System.Linq;
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
    public partial class EditUserWindow : Window
    {
        private readonly Guid _userId;

        // Declare the UserUpdated event
        public event EventHandler UserUpdated;

        public EditUserWindow(Guid userId)
        {
            InitializeComponent();
            _userId = userId;
            LoadUserData();
        }

        private async void LoadUserData()
        {
            try
            {
                var user = await FetchUserAsync(_userId);

                if (user != null)
                {
                    bool isAdmin = await IsUserAdmin(_userId);

                    if (isAdmin)
                    {
                        // Admin-specific logic: Load standard user data
                        NameTextBox.Text = user.Name;
                        NameKanjiTextBox.Text = user.NameKanji;
                        EmailTextBox.Text = user.Email;
                        PhoneNumberTextBox.Text = user.PhoneNumber;

                        // Preferred Language ComboBox
                        var preferredLanguageItem = PreferredLanguageComboBox.Items
                            .Cast<ComboBoxItem>()
                            .FirstOrDefault(item => (item.Tag as string) == user.PreferredLanguage);
                        if (preferredLanguageItem != null)
                        {
                            PreferredLanguageComboBox.SelectedItem = preferredLanguageItem;
                        }

                        // Role ComboBox
                        var roleItem = RoleComboBox.Items
                            .Cast<ComboBoxItem>()
                            .FirstOrDefault(item => item.Content.ToString() == "Admin");
                        if (roleItem != null)
                        {
                            RoleComboBox.SelectedItem = roleItem;
                        }

                        // Admins don't need location
                        LocationComboBox.IsEnabled = false;
                    }
                    else
                    {
                        await FetchLocations();
                        // Dealer-specific logic: Create DealerUserDTO from UserDTO
                        var dealerUser = new DealerUserDTO
                        {
                            Name = user.Name,
                            NameKanji = user.NameKanji,
                            Email = user.Email,
                            PhoneNumber = user.PhoneNumber,
                            PreferredLanguage = user.PreferredLanguage
                        };

                        // Fetch location for Dealer and set it
                        var location = await FetchLocationForDealer(_userId);
                        dealerUser.locationDTO = location;

                        if (dealerUser.locationDTO != null)
                        {
                            // Populate UI for Dealer
                            NameTextBox.Text = dealerUser.Name;
                            NameKanjiTextBox.Text = dealerUser.NameKanji;
                            EmailTextBox.Text = dealerUser.Email;
                            PhoneNumberTextBox.Text = dealerUser.PhoneNumber;

                            // Preferred Language ComboBox
                            var preferredLanguageItem = PreferredLanguageComboBox.Items
                                .Cast<ComboBoxItem>()
                                .FirstOrDefault(item => (item.Tag as string) == dealerUser.PreferredLanguage);
                            if (preferredLanguageItem != null)
                            {
                                PreferredLanguageComboBox.SelectedItem = preferredLanguageItem;
                            }

                            // Role ComboBox
                            RoleComboBox.SelectionChanged -= RoleComboBox_SelectionChanged;
                            var roleItem = RoleComboBox.Items
                                .Cast<ComboBoxItem>()
                                .FirstOrDefault(item => item.Content.ToString() == "Dealer");
                            if (roleItem != null)
                            {
                                RoleComboBox.SelectedItem = roleItem;
                            }
                            RoleComboBox.SelectionChanged += RoleComboBox_SelectionChanged;

                            // Enable and set location for Dealers
                            LocationComboBox.IsEnabled = true;
                            // Set SelectedItem based on LocationId
                            LocationComboBox.SelectedItem = LocationComboBox.Items
                          .Cast<LocationDTO>()
                          .FirstOrDefault(loc => loc.Id == dealerUser.locationDTO.Id);
                        }
                        else
                        {
                            MessageBox.Show("Nem található helyszín az adott kereskedőhöz.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Felhasználó nem található.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a felhasználó adatainak betöltésekor: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                this.Close();
            }
        }



        private async Task<LocationDTO> FetchLocationForDealer(Guid userId)
        {
            try
            {
                HttpResponseMessage response = await HttpClientService.Client.GetAsync($"/api/employeeLocations/{userId}");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<LocationDTO>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba történt a helyszín adatainak lekérésekor: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }


        private async Task<UserDTO> FetchUserAsync(Guid userId)
        {
            HttpResponseMessage response = await HttpClientService.Client.GetAsync($"/api/users/{userId}");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<UserDTO>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private async Task<bool> IsUserAdmin(Guid userId)
        {
            HttpResponseMessage response = await HttpClientService.Client.GetAsync($"/api/users/getPrivilege?userId={userId}");

            if (!response.IsSuccessStatusCode)
            {
                return false;
            }

            string jsonString = await response.Content.ReadAsStringAsync(); // Read JSON as a string
            var privileges = JsonSerializer.Deserialize<List<string>>(jsonString); // Deserialize manually

            if (privileges == null || privileges.Count == 0)
            {
                return false;
            }

            return privileges.Contains("Admin");
        }

        private async Task FetchLocations()
        {
            try
            {
                List<LocationDTO> locations = await FetchLocationsAsync();

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
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<LocationDTO>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private void RoleComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var selectedRole = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();

            if (selectedRole == "Dealer")
            {
                FetchLocations();
            }
            else
            {
                LocationComboBox.IsEnabled = false;
                LocationComboBox.SelectedItem = null;
            }
        }

        private async void UpdateUser_Click(object sender, RoutedEventArgs e)
        {
            string name = NameTextBox.Text;
            string nameKanji = NameKanjiTextBox.Text;
            string email = EmailTextBox.Text;
            string phoneNumber = PhoneNumberTextBox.Text;
            string preferredLanguage = (PreferredLanguageComboBox.SelectedItem as ComboBoxItem)?.Tag?.ToString();
            string role = (RoleComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            LocationDTO selectedLocation = LocationComboBox.SelectedItem as LocationDTO;

            if (string.IsNullOrWhiteSpace(phoneNumber) || !IsPhoneNumberValid(phoneNumber))
            {
                MessageBox.Show("Kérem adja meg érvényes telefonszámot.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (role == "Dealer" && selectedLocation == null)
            {
                MessageBox.Show("Kérem válasszon helyszínt.", "Hiba", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var userDto = new AdminUserUpdateDTO
            {
                Name = name,
                NameKanji = nameKanji,
                Email = email,
                PhoneNumber = phoneNumber,
                PreferredLanguage = preferredLanguage,
                Role = role,
                Location = role == "Dealer" ? selectedLocation : null
            };

            try
            {
                var json = JsonSerializer.Serialize(userDto, new JsonSerializerOptions { PropertyNamingPolicy = JsonNamingPolicy.CamelCase });
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                HttpResponseMessage response = await HttpClientService.Client.PutAsync($"/api/users/adminupdate/{_userId}", content);

                if (response.IsSuccessStatusCode)
                {
                    UserUpdated?.Invoke(this, EventArgs.Empty);
                    this.Close();
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

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

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
            if (!char.IsDigit(e.Text, 0) && e.Text != "+")
            {
                e.Handled = true;
            }
        }
    }

}