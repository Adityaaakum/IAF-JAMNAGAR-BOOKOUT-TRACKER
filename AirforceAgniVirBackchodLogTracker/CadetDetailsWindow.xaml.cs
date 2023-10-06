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
    /// Interaction logic for CadetDetailsWindow.xaml
    /// </summary>
    public partial class CadetDetailsWindow : Window
    {
        Cadet cadet;
        public CadetDetailsWindow(Cadet cadet)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            LoadImageBackground();
            this.cadet = cadet;
            PopulateDataTable();
        }

        private void Button_Click_Update(object sender, RoutedEventArgs e)
        {
            try
            {
                cadet.Name = NameTextBox.Text;
                cadet.Billet = BiletNumberTextBox.Text;
                cadet.MobileNumber = PhoneNumberTextBox.Text;
                cadet.ServiceNo = ServiceNumberTextBox.Text;
                cadet.Trade = TradeTextBox.Text;
                cadet.Unit = UnitTextBox.Text;
                cadet.VechileNumber = VehicleNumberTextBox.Text;
                cadet.MentorName = MentorNameTextBox.Text;
                cadet.MentorMobileNumber = MentoPhoneNumberTextBox.Text;

                if(cadet.MobileNumber.Length!=10 | cadet.MentorMobileNumber.Length != 10)
                {
                    throw  new Exception();
                }

                using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
                {
                    connection.CreateTable<Cadet>();
                    connection.Update(cadet);
                }                
                Close();
            }
            catch
            {
                MessageBox.Show("Something Went Wrong! Please Check the Details", "Failure",MessageBoxButton.OK, MessageBoxImage.Error);

            }
        }

        private void Button_Click_Delete(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to proceed?", "Confirmation", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {

                using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
                {
                    connection.CreateTable<Cadet>();
                    connection.Delete(cadet);
                }
                Close();
            }
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
            AgniveerDetails_Window.Background = imageBrush;

        }

        private void Button_Click_CheckoutCadate(object sender, RoutedEventArgs e)
        {
            CadetCheckOutWindow cadetCheckOutWindow= new CadetCheckOutWindow(cadet);
            cadetCheckOutWindow.ShowDialog();

            if (cadet.isBookedOut == 1)
            {
                CheckoutCadetWindow.IsEnabled = false;
            }
            

        }

        private void Button_Click_Cadet_Checkout_History(object sender, RoutedEventArgs e)
        {
            CadetCheckOutHistoryWindow cadetCheckOutHistoryWindow= new CadetCheckOutHistoryWindow(cadet);
            cadetCheckOutHistoryWindow.ShowDialog();
        }

        private void PopulateDataTable()
        {
            NameTextBox.Text= cadet.Name;
            BiletNumberTextBox.Text = cadet.Billet;
            PhoneNumberTextBox.Text = cadet.MobileNumber;
            ServiceNumberTextBox.Text = cadet.ServiceNo;
            TradeTextBox.Text = cadet.Trade;
            UnitTextBox.Text = cadet.Unit;
            VehicleNumberTextBox.Text = cadet.VechileNumber;
            MentorNameTextBox.Text = cadet.MentorName;
            MentoPhoneNumberTextBox.Text = cadet.MentorMobileNumber;

            if (cadet.isBookedOut == 1)
            {
                CheckoutCadetWindow.IsEnabled = false;
            }
            else
            {
                CheckoutCadetWindow.IsEnabled = true;
            }

           

        }
    }
}
