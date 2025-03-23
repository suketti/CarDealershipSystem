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