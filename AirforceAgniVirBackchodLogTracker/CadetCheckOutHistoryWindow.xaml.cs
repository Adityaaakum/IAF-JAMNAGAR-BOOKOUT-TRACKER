using AirforceAgniVirBackchodLogTracker.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Printing;
using System.Reflection;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Xml.Linq;

namespace AirforceAgniVirBackchodLogTracker
{
    /// <summary>
    /// Interaction logic for CadetCheckOutHistoryWindow.xaml
    /// </summary>
    public partial class CadetCheckOutHistoryWindow : Window
    {
        DataTable dataTable;
        List<BookOut> cadetBookOutList;
        Cadet cadet;
        public CadetCheckOutHistoryWindow(Cadet cadet)
        {
            InitializeComponent();
            WindowState = WindowState.Maximized;
            LoadImageBackground();
            this.cadet = cadet;
            dataTable = new DataTable();
            dataGrid_CheckoutHistory.ItemsSource = dataTable.DefaultView;
            
            cadetBookOutList = new List<BookOut>();
            PopulateDataTable();
            ValidateAddUserComboboxOptionsBasedOnUser();
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
                      DeleteBookOutHistory_Button.IsEnabled = false;
                    }
                }
            }
        }



        private void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {


            if (e.PropertyName == "PurposeOfVisit")
            {
                e.Column.Width = 900;
            }

            if (e.PropertyName == "TimeOut")
            {
                e.Column.Width = 300;
            }

            if (e.PropertyName == "TimeIn")
            {
                e.Column.Width = 300;
            }

            if (e.PropertyName == "Id")
            {
                e.Column.Visibility = Visibility.Hidden;
            }

            if (e.PropertyName == "UserID")
            {
                e.Column.Visibility = Visibility.Hidden;
            }


        }

        private void CreateDataTable()
        {
            dataTable.Columns.Clear();
            // Create columns based on the properties of the Person class
            foreach (var property in typeof(BookOut).GetProperties())
            {
                if (!(property.Name.Equals("Id") | property.Name.Equals("UserID")))
                {

                    dataTable.Columns.Add(property.Name, property.PropertyType);
                }

            }


        }

        private void PopulateDataTable()
        {
            CreateDataTable();
            ReadDatabase();
            dataTable.Rows.Clear();


            // Add data rows based on the list of Person objects
            foreach (var person in cadetBookOutList)
            {
                var row = dataTable.NewRow();
                foreach (var property in typeof(BookOut).GetProperties())
                {
                    if (!(property.Name.Equals("Id") | property.Name.Equals("UserID")))
                    {
                        row[property.Name] = property.GetValue(person);
                    }
                }
                dataTable.Rows.Add(row);
            }
            dataGrid_CheckoutHistory.ItemsSource = cadetBookOutList;
        }


        private void ReadDatabase()
        {
            using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
            {
                connection.CreateTable<BookOut>();
                cadetBookOutList = (connection.Table<BookOut>().Where(c => c.UserID == cadet.Id).ToList()).OrderBy(c => c.Id).ToList();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to proceed?", "Confirmation", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {
                using (SQLiteConnection connection = new SQLiteConnection(App.databasepath))
                {
                    connection.CreateTable<BookOut>();
                    var cadetBookOutList_ToBeDeleted = (connection.Table<BookOut>().Where(c => c.UserID == cadet.Id && c.TimeIn != null).ToList()).OrderBy(c => c.Id).ToList();
                    foreach (var record in cadetBookOutList_ToBeDeleted)
                    {
                        connection.Delete(record);
                    }

                }
                PopulateDataTable();
            }
        }

        private void PrintCheckoutHistory(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to proceed? This functionality may not work if this software is not compatible with Printer Driver", "Confirmation", MessageBoxButton.OKCancel);
            if (result == MessageBoxResult.OK)
            {

                FixedDocument document = LoadXamlDocument(GetXamlFilePath());


                PrintDialog printDialog = new PrintDialog();
                if (printDialog.ShowDialog() == true)
                {

                    printDialog.PrintQueue = new PrintQueue(new PrintServer(), printDialog.PrintQueue.FullName);
                    printDialog.PrintDocument(document.DocumentPaginator, "Print Job Name");
                }
            }

        }


        private FixedDocument LoadXamlDocument(string xamlPath)
        {
            using (FileStream stream = new FileStream(xamlPath, FileMode.Open, FileAccess.Read))
            {
                FixedDocument doc = (FixedDocument)XamlReader.Load(stream);
                return doc;
            }
        }

        public string GetXamlFilePath()
        {

            string executableDirectory = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().CodeBase);
            string actualPath = executableDirectory.Substring(0, executableDirectory.LastIndexOf("bin"));
            var parentDirectory = new Uri(actualPath).LocalPath;
            var xamlfileDirectory = System.IO.Path.Combine(parentDirectory, "PrintLayout", "PrintableContent.xaml");
            Console.WriteLine("PrintLayout Directory Path: " + xamlfileDirectory);


            if (File.Exists(xamlfileDirectory))
            {
                return xamlfileDirectory;
            }
            else
            {
                throw new FileNotFoundException("printlayout.xml not found.");
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
            CheckOutHistory.Background = imageBrush;

        }


        private void GeneratePdf(Grid grid)
        {

            //PdfDocument document = new PdfDocument();
            //document.Info.Title = "Grid to PDF";

            //// Add a page to the document
            //PdfPage page = document.AddPage();

            //// Create a graphics object for drawing on the page
            //XGraphics gfx = XGraphics.FromPdfPage(page);

            //// Define a PDF rectangle that matches the size of the Grid
            //XRect pdfRect = new XRect(0, 0, page.Width, page.Height);

            //// Convert XRect to Rect
            //double pdfToWpfFactor = 96.0 / 72.0; // Convert PDF units to WPF units (usually pixels)
            //System.Windows.Rect wpfRect = new System.Windows.Rect(pdfRect.Left * pdfToWpfFactor,
            //                                                     pdfRect.Top * pdfToWpfFactor,
            //                                                     pdfRect.Width * pdfToWpfFactor,
            //                                                     pdfRect.Height * pdfToWpfFactor);

            //// Render the Grid to a bitmap
            //RenderTargetBitmap renderTargetBitmap = new RenderTargetBitmap((int)pdfRect.Width, (int)pdfRect.Height, 96, 96, System.Windows.Media.PixelFormats.Pbgra32);
            //renderTargetBitmap.Render(grid);

            //// Convert the bitmap to a PDF image
            //MemoryStream imageStream = new MemoryStream();
            //BmpBitmapEncoder encoder = new BmpBitmapEncoder();
            //encoder.Frames.Add(BitmapFrame.Create(renderTargetBitmap));
            //encoder.Save(imageStream);
            //XImage pdfImage = XImage.FromStream(imageStream);

            //// Draw the image onto the PDF page
            //gfx.DrawImage(pdfImage, pdfRect);

            //// Save the PDF to a file or memory stream
            //string pdfFilePath = "AgniVeer_Book_Out_History"+DateTime.Now.ToString("dd-MM-yyyy h:mm tt") +".pdf";

            //using (FileStream fs = new FileStream(pdfFilePath, FileMode.Create))
            //{
            //    // Specify UTF-8 encoding when saving the PDF
            //    document.Save(fs, false, Encoding.UTF8);
            //}


            //// Optionally, open the PDF file with the default PDF viewer
            //System.Diagnostics.Process.Start(pdfFilePath);

        }
    }
}
