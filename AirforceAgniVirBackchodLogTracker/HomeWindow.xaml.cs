using AirforceAgniVirBackchodLogTracker.Model;
using IAF_JAMNAGAR_BOOKOUT_TRACKER;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
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
    /// Interaction logic for HomeWindow.xaml
    /// </summary>
    public partial class HomeWindow : Window
    {
        List<Cadet> cadetList;
        List<Cadet> bookedOutCadetList;
        DataTable dataTable;
        public HomeWindow()
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            cadetList = new List<Cadet>();
            dataTable = new DataTable();
            bookedOutCadetList = new List<Cadet>();
            dataGrid.ItemsSource = dataTable.DefaultView;
            dataGrid_BookOutCadets.ItemsSource = dataTable.DefaultView;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            LoadImageBackground();
            PopulateDataTable();

        }


        public void ReadDatabase(List<Cadet> filteredCadetListGrid1 = null, List<Cadet> filteredCadetListGrid2 = null)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
            {


                if (filteredCadetListGrid1 != null)
                {
                    connection.CreateTable<Cadet>();
                    cadetList = filteredCadetListGrid1;
                    bookedOutCadetList = connection.Table<Cadet>().Where(c => c.isBookedOut == 1).ToList().OrderBy(c => c.Name).ToList();
                    return;

                }

                if (filteredCadetListGrid2 != null)
                {
                    connection.CreateTable<Cadet>();
                    cadetList = (connection.Table<Cadet>().ToList()).OrderBy(c => c.Name).ToList();
                    bookedOutCadetList = filteredCadetListGrid2;
                    return;

                }

                connection.CreateTable<Cadet>();
                cadetList = (connection.Table<Cadet>().ToList()).OrderBy(c => c.Name).ToList();
                bookedOutCadetList = connection.Table<Cadet>().Where(c => c.isBookedOut == 1).ToList().OrderBy(c => c.Name).ToList();
            }

        }

        private void PopulateDataTable(List<Cadet> filteredCadetListGrid1 = null, List<Cadet> filteredCadetListGrid2 = null)
        {
            CreateDataTable();
            ReadDatabase(filteredCadetListGrid1, filteredCadetListGrid2);
            dataTable.Rows.Clear();


            // Add data rows based on the list of Person objects
            foreach (var person in cadetList)
            {
                var row = dataTable.NewRow();
                foreach (var property in typeof(Cadet).GetProperties())
                {
                    if (!(property.Name.Equals("Id") | property.Name.Equals("isBookedOut")))
                    {
                        row[property.Name] = property.GetValue(person);
                    }
                }
                dataTable.Rows.Add(row);
            }
            foreach (var person in bookedOutCadetList)
            {
                var row = dataTable.NewRow();
                foreach (var property in typeof(Cadet).GetProperties())
                {
                    if (!(property.Name.Equals("Id") | property.Name.Equals("isBookedOut")))
                    {
                        row[property.Name] = property.GetValue(person);
                    }
                }
                dataTable.Rows.Add(row);
            }
            dataGrid.ItemsSource = cadetList;
            dataGrid_BookOutCadets.ItemsSource = bookedOutCadetList;
        }


        private void CreateDataTable()
        {
            dataTable.Columns.Clear();
            // Create columns based on the properties of the Person class
            foreach (var property in typeof(Cadet).GetProperties())
            {
                if (!(property.Name.Equals("Id") | property.Name.Equals("isBookedOut")))
                {


                    dataTable.Columns.Add(property.Name, property.PropertyType);
                }

            }

        }





        private void Button_Click(object sender, RoutedEventArgs e)
        {
            AddCadetWindow addCadetWindow = new AddCadetWindow();
            addCadetWindow.ShowDialog();
            PopulateDataTable();


        }

        private void dataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid.SelectedItem != null)
            {
                // Get the selected item (row) from the DataGrid
                var selectedPerson = dataGrid.SelectedItem as Cadet;
                CadetDetailsWindow cadetDetailsWindow = new CadetDetailsWindow(selectedPerson);
                cadetDetailsWindow.ShowDialog();
                PopulateDataTable();

            }
        }

        private void dataGrid_BookOutCadets_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (dataGrid_BookOutCadets.SelectedItem != null)
            {
                // Get the selected item (row) from the DataGrid
                var selectedPerson = dataGrid_BookOutCadets.SelectedItem as Cadet;
                CadetCheckInWindow cadetCheckInWindow = new CadetCheckInWindow(selectedPerson);
                cadetCheckInWindow.ShowDialog();
                PopulateDataTable();

            }

        }

        private void LoadImageBackground()
        {
            ImageBrush imageBrush = new ImageBrush();
            string executableDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            string actualPath = executableDirectory.Substring(0, executableDirectory.LastIndexOf("bin"));
            var parentDirectory = new Uri(actualPath).LocalPath;
            var WallpaperDirectory = System.IO.Path.Combine(parentDirectory, "Images", "Indian-Air-Force.jpeg");
            imageBrush.ImageSource = new BitmapImage(new Uri(WallpaperDirectory, UriKind.RelativeOrAbsolute));
            imageBrush.Stretch = Stretch.Uniform;
            imageBrush.Opacity = 0.5;
            MainWindow_Grid.Background = imageBrush;

        }



        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            if (e.PropertyName == "Name")
            {
                e.Column.Width = 200;
            }

            if (e.PropertyName == "ServiceNo")
            {
                e.Column.Width = 200;
            }

            if (e.PropertyName == "Trade")
            {
                e.Column.Width = 200;
            }

            if (e.PropertyName == "MobileNumber")
            {
                e.Column.Width = 200;
            }

            if (e.PropertyName == "VechileNumber")
            {
                e.Column.Width = 200;
            }

            if (e.PropertyName == "Id")
            {
                e.Column.Visibility = Visibility.Hidden;
            }

            if (e.PropertyName == "isBookedOut")
            {
                e.Column.Visibility = Visibility.Hidden;
            }
        }

        private void TextBox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox.Text == "")
            {
                PopulateDataTable();
            }
            var filteredList = cadetList.Where(c => c.Name.ToLower().Contains(textBox.Text.ToLower())).ToList();
            PopulateDataTable(filteredList);

        }

        private void TextBox_SelectionChanged_1(object sender, RoutedEventArgs e)
        {
            TextBox textBox = sender as TextBox;

            if (textBox.Text == "")
            {
                PopulateDataTable();
            }
            var filteredList = bookedOutCadetList.Where(c => c.Name.ToLower().Contains(textBox.Text.ToLower())).ToList();
            PopulateDataTable(null,filteredList);


        }

        private void AddUserClick(object sender, RoutedEventArgs e)
        {

            AddUserWindow addUserWindow = new AddUserWindow();
            addUserWindow.ShowDialog();
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
            {
                connection.CreateTable<User>();
                var userList = (connection.Table<User>().ToList()).OrderBy(c => c.username).ToList();
                var user = userList.Where(u => u.IsLogged == true).ToList();

                foreach (var userLogged in user)
                {
                    userLogged.IsLogged = false;
                    connection.Update(userLogged);
                }

            }
        }

        private void ChangePassword_Click(object sender, RoutedEventArgs e)
        {
            ChangePasswordWindow changePasswordWindow= new ChangePasswordWindow();
            changePasswordWindow.ShowDialog();

        }
    }
}
