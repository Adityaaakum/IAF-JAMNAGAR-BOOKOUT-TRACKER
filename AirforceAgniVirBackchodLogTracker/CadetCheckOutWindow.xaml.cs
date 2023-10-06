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
    /// Interaction logic for CadetCheckOutWindow.xaml
    /// </summary>
    public partial class CadetCheckOutWindow : Window
    {
        Cadet cadet;
        public CadetCheckOutWindow(Cadet cadet)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            LoadImageBackground();
            this.cadet = cadet;
            PopulateDataTable();
        }

        private void PopulateDataTable()
        {
            NameTextBox.Text = cadet.Name;
            PurposeOfVisitTextBox.Text = "Please Enter the Purpose of Visit";
            CheckOutTimeTextBox.Text= DateTime.Now.ToString("dd-MM-yyyy h:mm tt");
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
            CadetCheckOutWindow_Grid.Background = imageBrush;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to Checkout?", "Confirmation", MessageBoxButton.OKCancel);

            if (result == MessageBoxResult.OK)
            {

                BookOut bookout = new BookOut();
                bookout.UserID = cadet.Id;
                bookout.PurposeOfVisit = PurposeOfVisitTextBox.Text;
                bookout.TimeOut = CheckOutTimeTextBox.Text;
                cadet.isBookedOut = 1;
                using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
                {
                    connection.CreateTable<BookOut>();
                    connection.Insert(bookout);

                }

                using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
                {
                    connection.CreateTable<Cadet>();
                    connection.Update(cadet);
                }
                Close();
            }

        }
    }
}
