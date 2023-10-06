using AirforceAgniVirBackchodLogTracker.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace AirforceAgniVirBackchodLogTracker
{
    /// <summary>
    /// Interaction logic for AddCadetWindow.xaml
    /// </summary>
    public partial class AddCadetWindow : Window
    {
        public AddCadetWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            LoadImageBackground();
        }

        private void LoadImageBackground()
        {
            ImageBrush imageBrush = new ImageBrush();
            string executableDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            string actualPath = executableDirectory.Substring(0, executableDirectory.LastIndexOf("bin"));
            var parentDirectory = new Uri(actualPath).LocalPath;
            var WallpaperDirectory = System.IO.Path.Combine(parentDirectory, "Images", "IAF.jpg");
            imageBrush.ImageSource = new BitmapImage(new Uri(WallpaperDirectory, UriKind.RelativeOrAbsolute));
            imageBrush.Stretch = Stretch.Uniform;
            imageBrush.Opacity = 0.5;
            AddCadet_Grid.Background = imageBrush;

        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Cadet cadet= new Cadet();  

                cadet.Name=NameTextBox.Text;
                cadet.Billet=BiletNumberTextBox.Text;
                cadet.Unit=UnitTextBox.Text;
                cadet.ServiceNo = ServiceNumberTextBox.Text;
                cadet.Trade=TradeTextBox.Text;
                cadet.VechileNumber=VehicleNumberTextBox.Text;

                if(PhoneNumberTextBox.Text.Length==10)
                cadet.MobileNumber = PhoneNumberTextBox.Text;

                else
                    throw new Exception();

                cadet.MentorName=MentorNameTextBox.Text;
                if (!(MentoPhoneNumberTextBox.Text.Length == 10))
                {
                    
                    throw new Exception();
                }

                else
                    cadet.MentorMobileNumber = MentoPhoneNumberTextBox.Text;

                using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
                { if((PhoneNumberTextBox.Text.Length == 10 && MentoPhoneNumberTextBox.Text.Length == 10 && ServiceNumberTextBox.Text!=null))
                    connection.CreateTable<Cadet>();
                    connection.Insert(cadet);
                }
                Close();


            }
            catch {
                MessageBox.Show("Something Went Wrong! Please Check the details again", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

       
    }
}
