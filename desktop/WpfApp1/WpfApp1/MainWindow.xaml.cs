using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System.Windows.Input;




namespace WpfApp1
{
    public partial class MainWindow : Window
    {
        public ObservableCollection<Car> autok { get; set; } = new ObservableCollection<Car>();
        private Car tempCar; // Ideiglenes változat a szerkesztéshez

        public MainWindow()
        {
            InitializeComponent();
            AutoListView.ItemsSource = autok; // Biztosítsd, hogy az elején is működik
            ShowLoginWindow();
            this.DataContext = this;
        }


        private void ShowLoginWindow()
        {
            LoginWindow loginWindow = new LoginWindow();
            if (loginWindow.ShowDialog() != true)
            {
                this.Close();
            }
        }

        private void lblOsszesAuto_MouseDown(object sender, MouseButtonEventArgs e)
        {
            Dispatcher.Invoke(() =>
            {
                autok.Clear(); // Előző adatokat töröljük

                autok.Add(new Car { Kep = "https://via.placeholder.com/80", Marka = "Toyota", Modell = "Corolla", Ar = "3 500 000 Ft", Uzemanyag = "Benzin", Meghajtas = "Elsőkerék", Szin = "Fehér", MotorMeret = "1.6 L", KmAllas = "85 000 km", Evjarat = "2020", Kivitel = "Sedan" });

                autok.Add(new Car { Kep = "https://via.placeholder.com/80", Marka = "Ford", Modell = "Focus", Ar = "2 900 000 Ft", Uzemanyag = "Dízel", Meghajtas = "Elsőkerék", Szin = "Kék", MotorMeret = "1.5 L", KmAllas = "65 000 km", Evjarat = "2019", Kivitel = "Hatchback" });

                AutoListView.ItemsSource = autok; // Frissítés
                AutoListView.Visibility = Visibility.Visible; // Megjelenítés
            });
        }









        private void DeleteCar(object sender, RoutedEventArgs e)
        {
            Button deleteButton = sender as Button;
            if (deleteButton != null)
            {
                Car selectedCar = (Car)deleteButton.DataContext;
                Dispatcher.Invoke(() => autok.Remove(selectedCar)); // A UI Thread kezeli
            }
        }






        private void EditCar(object sender, RoutedEventArgs e)
        {
            Button editButton = sender as Button;
            if (editButton != null)
            {
                Car selectedCar = (Car)editButton.DataContext;

                // Ideiglenes másolat az eredeti adatok mentéséhez
                tempCar = new Car
                {
                    Kep = selectedCar.Kep,
                    Marka = selectedCar.Marka,
                    Modell = selectedCar.Modell,
                    Ar = selectedCar.Ar,
                    Uzemanyag = selectedCar.Uzemanyag,
                    Meghajtas = selectedCar.Meghajtas,
                    Szin = selectedCar.Szin,
                    MotorMeret = selectedCar.MotorMeret,
                    KmAllas = selectedCar.KmAllas,
                    Evjarat = selectedCar.Evjarat,
                    Kivitel = selectedCar.Kivitel
                };

                selectedCar.IsEditing = true;
                AutoListView.Items.Refresh();
            }
        }

        private void SaveCar(object sender, RoutedEventArgs e)
        {
            Button saveButton = sender as Button;
            if (saveButton != null)
            {
                Car selectedCar = (Car)saveButton.DataContext;
                selectedCar.IsEditing = false;
                AutoListView.Items.Refresh();
            }
        }

        private void CancelEdit(object sender, RoutedEventArgs e)
        {
            Button cancelButton = sender as Button;
            if (cancelButton != null)
            {
                Car selectedCar = (Car)cancelButton.DataContext;

                // Ha történt változás, akkor kérdezzen rá a felhasználóra
                if (selectedCar.Marka != tempCar.Marka || selectedCar.Modell != tempCar.Modell || selectedCar.Ar != tempCar.Ar)
                {
                    MessageBoxResult result = MessageBox.Show("Biztosan visszavonja a módosításokat?", "Mégse szerkesztés", MessageBoxButton.YesNo, MessageBoxImage.Question);
                    if (result == MessageBoxResult.No)
                    {
                        return;
                    }
                }

                // Visszaállítjuk az eredeti adatokat
                selectedCar.Marka = tempCar.Marka;
                selectedCar.Modell = tempCar.Modell;
                selectedCar.Ar = tempCar.Ar;
                selectedCar.Uzemanyag = tempCar.Uzemanyag;
                selectedCar.Meghajtas = tempCar.Meghajtas;
                selectedCar.Szin = tempCar.Szin;
                selectedCar.MotorMeret = tempCar.MotorMeret;
                selectedCar.KmAllas = tempCar.KmAllas;
                selectedCar.Evjarat = tempCar.Evjarat;
                selectedCar.Kivitel = tempCar.Kivitel;

                selectedCar.IsEditing = false;
                AutoListView.Items.Refresh();
            }
        }
    }

    public class Car : INotifyPropertyChanged
    {
        private bool _isEditing;
        public bool IsEditing
        {
            get { return _isEditing; }
            set { _isEditing = value; OnPropertyChanged(nameof(IsEditing)); }
        }

        public string Kep { get; set; }
        public string Marka { get; set; }
        public string Modell { get; set; }
        public string Ar { get; set; }
        public string Uzemanyag { get; set; }
        public string Meghajtas { get; set; }
        public string Szin { get; set; }
        public string MotorMeret { get; set; }
        public string KmAllas { get; set; }
        public string Evjarat { get; set; }
        public string Kivitel { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }


}
