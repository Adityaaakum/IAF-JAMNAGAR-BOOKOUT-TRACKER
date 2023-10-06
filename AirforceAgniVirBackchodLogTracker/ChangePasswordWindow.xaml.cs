using AirforceAgniVirBackchodLogTracker.Model;
using AirforceAgniVirBackchodLogTracker;
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

namespace IAF_JAMNAGAR_BOOKOUT_TRACKER
{
    /// <summary>
    /// Interaction logic for ChangePasswordWindow.xaml
    /// </summary>
    public partial class ChangePasswordWindow : Window
    {
        public ChangePasswordWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            LoadImageBackground();
        }

        private void Change_Password_Click(object sender, RoutedEventArgs e)
        {
            try {
                if (password_Box.Password != "" && verify_password_Box.Password != "")
                {
                    if (password_Box.Password.Equals(verify_password_Box.Password))
                    {

                        MessageBoxResult result = MessageBox.Show("Are you sure you want to Procced?", "Confirmation", MessageBoxButton.OKCancel);

                        if (result == MessageBoxResult.OK)
                        {
                            using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
                            {
                                connection.CreateTable<User>();
                                var userList = (connection.Table<User>().ToList()).OrderBy(c => c.username).ToList();
                                var user = userList.Where(u => u.IsLogged == true).First();
                                if (user.password.Equals(password_Box.Password))
                                {
                                    MessageBox.Show("Something Went Wrong! Your new password cannot be same as your old password. Please choose a different password.", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                                    return;
                                }

                                user.password = password_Box.Password;
                                connection.Update(user);
                                MessageBox.Show("Password Changed Successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

                            }
                        }

                    }
                    else
                    {
                        MessageBox.Show("Something Went Wrong! Password and Verify Password do not match. Please make sure they are identical.", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                    }

                }
                else
                {
                    MessageBox.Show("Something Went Wrong! Please Verify if you have entered all the required fields", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            }
            catch
            {
                MessageBox.Show("Something Went Wrong! Password Cannot be Changed Please contact System Developer or System Admin for further Details", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);
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
            AccountReset_Window.Background = imageBrush;
        }

    }
}

