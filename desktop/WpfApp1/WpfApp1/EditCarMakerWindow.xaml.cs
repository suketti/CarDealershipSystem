using System.Windows;
using WpfApp1.Models;

namespace WpfApp1
{
    public partial class EditCarMakerWindow : Window
    {
        public CarMakerDTO UpdatedCarMaker { get; private set; }

        public EditCarMakerWindow(CarMakerDTO carMaker)
        {
            InitializeComponent();
            UpdatedCarMaker = carMaker;
            BrandEnglishTextBox.Text = carMaker.BrandEnglish;
            BrandJapaneseTextBox.Text = carMaker.BrandJapanese;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            // Validate that both fields are filled
            if (string.IsNullOrEmpty(BrandEnglishTextBox.Text) || string.IsNullOrEmpty(BrandJapaneseTextBox.Text))
            {
                MessageBox.Show("A márka angol és japán nevét is ki kell tölteni!", "Hiba", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // If both fields are filled, proceed with saving the data
            UpdatedCarMaker.BrandEnglish = BrandEnglishTextBox.Text;
            UpdatedCarMaker.BrandJapanese = BrandJapaneseTextBox.Text;

            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}