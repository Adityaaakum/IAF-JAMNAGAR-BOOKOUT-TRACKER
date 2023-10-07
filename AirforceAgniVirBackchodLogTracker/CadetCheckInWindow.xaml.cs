using AirforceAgniVirBackchodLogTracker.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.DirectoryServices.ActiveDirectory;
using System.Globalization;
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
    /// Interaction logic for CadetCheckInWindow.xaml
    /// </summary>
    public partial class CadetCheckInWindow : Window
    {
        Cadet cadet;
        BookOut bookout;
        public CadetCheckInWindow(Cadet cadet)
        {
            InitializeComponent();            
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            LoadImageBackground();
            this.cadet = cadet;
            PopulateDataTable();
        }

        private void PopulateDataTable()
        { 
            GetBookOutDetails();
            NameTextBox.Text = cadet.Name;
            PurposeOfVisitTextBox.Text = bookout.PurposeOfVisit;
            CheckOutTimeTextBox.Text = bookout.TimeOut;
            CheckInTimeTextBox.Text= DateTime.Now.ToString("dd-MM-yyyy h:mm tt");
            
        }

        private BookOut GetBookOutDetails()
        {           
            
            using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
            {
                connection.CreateTable<BookOut>();
                 bookout = connection.Table<BookOut>().LastOrDefault(b=>b.UserID==cadet.Id);
                
            }
            return bookout;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to proceed?", "Confirmation", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                bookout.TimeIn= DateTime.Now.ToString("dd-MM-yyyy h:mm tt");
                using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
                {
                    connection.CreateTable<BookOut>();
                    connection.Update(bookout);
                }
                cadet.isBookedOut = 0;
                var totalTime=CalculateTotalTime();
                if( !IsPost10PM() && totalTime>24)
                {
                    cadet.TotalLateEntries += 1;
                }
                using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
                {
                    connection.CreateTable<Cadet>();
                    connection.Update(cadet);
                }
            }
            Close();
        }

        private Double CalculateTotalTime()
        {
            DateTime startTime = DateTime.ParseExact(bookout.TimeOut, "dd-MM-yyyy h:mm tt", CultureInfo.InvariantCulture);
            DateTime endTime = DateTime.ParseExact(bookout.TimeIn, "dd-MM-yyyy h:mm tt", CultureInfo.InvariantCulture);
            TimeSpan timeDifference = endTime - startTime;           
            double hoursDifference = timeDifference.TotalHours;

            return hoursDifference;
        }

        private bool IsPost10PM()
        {           
            DateTime currentTime = DateTime.Now;          
            DateTime tenPM = currentTime.Date.AddHours(22);             
            if (currentTime >= tenPM)
            {
                return true; 
            }
            else
            {
                return false; 
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
            CadetCheckIn_Grid.Background = imageBrush;

        }
    }
}
