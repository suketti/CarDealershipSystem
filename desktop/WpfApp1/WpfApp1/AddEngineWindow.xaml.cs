using System.Collections.Generic;
using System.Windows;
using WpfApp1.Models;

namespace WpfApp1.Views
{
    public partial class AddEngineWindow : Window
    {
        public EngineSizeModelDTO NewEngine { get; private set; }

        public AddEngineWindow(List<FuelTypeDTO> fuelTypes)
        {
            InitializeComponent();
            cbFuelTypes.ItemsSource = fuelTypes;
        }

        private void btnAddEngine_Click(object sender, RoutedEventArgs e)
        {
            NewEngine = new EngineSizeModelDTO
            {
                EngineSize = int.Parse(tbEngineSize.Text),
                FuelType = new FuelTypeDTO
                {
                    ID = (int)cbFuelTypes.SelectedValue,
                    NameEnglish = ((FuelTypeDTO)cbFuelTypes.SelectedItem).NameEnglish,
                    NameJapanese = ((FuelTypeDTO)cbFuelTypes.SelectedItem).NameJapanese
                }
            };

            DialogResult = true;
            Close();
        }
    }
}