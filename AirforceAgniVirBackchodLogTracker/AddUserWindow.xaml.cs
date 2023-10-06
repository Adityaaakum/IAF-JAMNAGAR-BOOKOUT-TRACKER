using AirforceAgniVirBackchodLogTracker.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Runtime.InteropServices;
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
    /// Interaction logic for AddUserWindow.xaml
    /// </summary>
    public partial class AddUserWindow : Window
    {
        string user_permission;
        public AddUserWindow()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;
            LoadImageBackground();
            ValidateAddUserComboboxOptionsBasedOnUser();
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
            AddUser_Grid.Background = imageBrush;

        }

        private void ValidateAddUserComboboxOptionsBasedOnUser()
        {
            

            using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
            {
                connection.CreateTable<User>();
                var user = connection.Table<User>().Where(u => u.IsLogged == true).First();
                
                if (user != null)
                {
                    if (!user.UserType.Equals("ADMIN"))
                    {
                        foreach( ComboBoxItem ComboBoxitem in comboBox.Items)
                        {
                            if (ComboBoxitem.Content.ToString() == "Admin")
                            {
                                ComboBoxitem.IsEnabled=false;
                                break;
                            }
                        }

                    }
                }

            }
        }

        private void comboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;
            ComboBoxItem selectedComboBoxItem = (ComboBoxItem)comboBox.SelectedItem;

            if (selectedComboBoxItem != null)
            {
                user_permission = selectedComboBoxItem.Content.ToString();

                    if (user_permission.ToLower().Equals("admin"))
                    {
                        user_permission = "ADMIN";
                    }
                    else
                    {
                        user_permission = "STAFF";
                    }
            
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (username_TextBox.Text != "" && user_passwordbox.Password != "" && user_passwordbox_Verify.Password != "" && user_permission != null)
                {
                    if (user_passwordbox.Password.Equals(user_passwordbox_Verify.Password))
                    {
                        User user = new User();

                        user.username = username_TextBox.Text;
                        user.password = user_passwordbox.Password;
                        user.UserType = user_permission;
                        user.IsLogged = false;
                        using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
                        {
                            connection.CreateTable<User>();
                            var userList = (connection.Table<User>().ToList()).OrderBy(c => c.username).ToList();

                            foreach (var userpresent in userList)
                            {
                                if (userpresent.username.Equals(username_TextBox.Text))
                                {
                                    throw new Exception();
                                }
                            }
                            if (connection.Insert(user) == 1)
                            {
                                MessageBox.Show("Congratulations! Your account has been successfully registered. You can now log in with your username and password.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                            }
                            else
                            {
                                MessageBox.Show("Something Went Wrong! Please Contact your System Administrator for more Details", "Success", MessageBoxButton.OK, MessageBoxImage.Information);

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
                MessageBox.Show("Something Went Wrong! The username you entered is already in use. Please choose a different username.", "Failure", MessageBoxButton.OK, MessageBoxImage.Error);

            }

        }
    }
}
