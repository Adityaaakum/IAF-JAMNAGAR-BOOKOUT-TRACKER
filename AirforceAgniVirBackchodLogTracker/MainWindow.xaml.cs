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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace AirforceAgniVirBackchodLogTracker
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        List<User> userList;
        public MainWindow()
        {
            InitializeComponent();
            this.ResizeMode = ResizeMode.NoResize;
            LoadImageBackground();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            AddDefaultUser();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {


            using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
            {               
                
                if (UsernameTextBox.Text != "" && PasswordBox.Password != "")
                {
                    if (AuthenticateUser(UsernameTextBox.Text, PasswordBox.Password))
                    {
                        HomeWindow homeWindow = new HomeWindow();
                        this.Close();
                        homeWindow.Show();
                    }

                    else
                    {
                        MessageBox.Show("Something Went Wrong! Please Check your login details", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);

                    }
                }
                    else
                    {
                        MessageBox.Show("Something Went Wrong! Please Check your Login Details", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);

                    }


                
            }

        }

        private void LoadImageBackground()
        {
            ImageBrush imageBrush = new ImageBrush();
            string executableDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            string actualPath = executableDirectory.Substring(0, executableDirectory.LastIndexOf("bin"));
            var parentDirectory = new Uri(actualPath).LocalPath;
            var WallpaperDirectory = System.IO.Path.Combine(parentDirectory, "Images", "IAF_HomePage_Brush.jpg");
            imageBrush.ImageSource = new BitmapImage(new Uri(WallpaperDirectory, UriKind.RelativeOrAbsolute));
            imageBrush.Stretch = Stretch.UniformToFill;
            MainWindowGrid.Background = imageBrush;
        }

        private bool AuthenticateUser(string username, string password)
        {
            try
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
                {
                    connection.CreateTable<User>();
                    userList = (connection.Table<User>().ToList()).OrderBy(c => c.username).ToList();
                    var user = userList.Where(u => u.username.Equals(username)).First();

                    if (user != null)
                    {
                        if (user.password.Equals(password))
                        {
                            user.IsLogged = true;
                            connection.Update(user);
                            return true;
                        }
                    }

                    return false;
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show("Something Went Wrong! Please Check your Login Details", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }
        }

        private void AddDefaultUser()
        {          
            try
            {
                User user = new User()
                {
                    username = "Admin_IAF_Jamnagar",
                    password = "Test.1234",
                    UserType = "ADMIN",
                    IsLogged = false
                };
                using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
                {
                    userList = (connection.Table<User>().ToList()).OrderBy(c => c.username).ToList();
                    if (userList.Count == 0)
                    {
                        connection.CreateTable<User>();
                        connection.Insert(user);
                    }

                }
            }
            catch
            {
                MessageBox.Show("Something Went Wrong! Please Login Later", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }

        private void Hyperlink_Click(object sender, RoutedEventArgs e)
        {
            var hyperlink = (Hyperlink)sender;
            Uri uri = hyperlink.NavigateUri;
            MessageBox.Show("System Developer has disabled User Registration. Please Contact Developer or System Admin to Create new user", "Failure", MessageBoxButton.OK, MessageBoxImage.Information);

        }

        
    }
}
