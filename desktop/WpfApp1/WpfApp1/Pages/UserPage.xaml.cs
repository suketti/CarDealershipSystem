using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using WpfApp1.Models;
using WpfApp1.Services;

namespace WpfApp1
{
    /// <summary>
    /// UserPage.xaml interaction logic
    /// </summary>
    public partial class UserPage : Page
    {
        public UserPage()
        {
            InitializeComponent();
            LoadUsersAsync();
        }

        private async void LoadUsersAsync()
        {
            try
            {
                var users = await FetchUsersAsync();
                UsersDataGrid.ItemsSource = users;  
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Hiba a felhasználók betöltése közben: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private async Task<List<UserDTO>> FetchUsersAsync()
        {
            HttpResponseMessage response = await HttpClientService.Client.GetAsync("/api/users");
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return JsonSerializer.Deserialize<List<UserDTO>>(responseBody, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        private void AddUser_Click(object sender, RoutedEventArgs e)
        {
            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.UserAdded += AddUserWindow_UserAdded;
            addUserWindow.ShowDialog();
        }

        private void AddUserWindow_UserAdded(object sender, EventArgs e)
        {
            LoadUsersAsync();
        }

        private void EditUser_Click(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            if (editButton != null)
            {
                UserDTO selectedUser = editButton.DataContext as UserDTO;
                if (selectedUser != null)
                {
                    // Create the EditUserWindow and pass the selected user's ID
                    EditUserWindow editUserWindow = new EditUserWindow(selectedUser.ID);

                    // Optionally, subscribe to the UserUpdated event to refresh the data after the edit
                    editUserWindow.UserUpdated += (s, args) =>
                    {
                        // Refresh data or take necessary action after the user has been updated
                        MessageBox.Show("A felhasználó sikeresen frissült!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                    };

                    // Show the EditUserWindow
                    editUserWindow.ShowDialog();  // Use ShowDialog to make it modal (blocking) if needed
                }
            }
        }


        private void EditUserWindow_UserEdited(object sender, EventArgs e)
        {
            LoadUsersAsync();
        }

        private async void DeleteUser_Click(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                UserDTO selectedUser = deleteButton.DataContext as UserDTO;
                if (selectedUser != null)
                {
                    Guid currentUserId = selectedUser.ID;

                    // Check if the current user is an Admin
                    bool isAdmin = await IsUserAdmin(currentUserId);

                    // Decide the API route based on whether the user is an Admin
                    string apiRoute = isAdmin ? "/api/users/delete" : "/api/users/delete-dealer";

                    // Confirmation message
                    var result = MessageBox.Show($"Biztosan törölni akarod a következő felhasználót: {selectedUser.Name}?",
                                                  "Törlés megerősítése",
                                                  MessageBoxButton.YesNo,
                                                  MessageBoxImage.Warning);

                    if (result == MessageBoxResult.Yes)
                    {
                        try
                        {
                            // Prepare the request body for the DELETE request
                            var requestBody = new
                            {
                                id = selectedUser.ID.ToString(),  // Ensure that the ID is passed as a string
                                name = selectedUser.Name,
                                nameKanji = selectedUser.NameKanji,
                                email = selectedUser.Email,
                                userName = selectedUser.UserName,
                                phoneNumber = selectedUser.PhoneNumber,
                                preferredLanguage = selectedUser.PreferredLanguage
                            };

                            // Debugging: Check the request body before sending
                            string jsonBody = JsonSerializer.Serialize(requestBody);
                            Console.WriteLine("Request Body: " + jsonBody);

                            // Send the DELETE request to the correct endpoint
                            HttpResponseMessage response = await HttpClientService.Client.SendAsync(new HttpRequestMessage
                            {
                                Method = HttpMethod.Delete,
                                RequestUri = new Uri(HttpClientService.Client.BaseAddress, apiRoute),
                                Content = new StringContent(jsonBody, System.Text.Encoding.UTF8, "application/json")
                            });

                            // Log response for debugging
                            string responseContent = await response.Content.ReadAsStringAsync();
                            Console.WriteLine("Response: " + responseContent);

                            // Ensure success
                            response.EnsureSuccessStatusCode();

                            MessageBox.Show("A felhasználó sikeresen törölve lett!", "Siker", MessageBoxButton.OK, MessageBoxImage.Information);
                            LoadUsersAsync();
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show($"Hiba a felhasználó törlése közben: {ex.Message}", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                        }
                    }
                }
            }
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

    }
}
