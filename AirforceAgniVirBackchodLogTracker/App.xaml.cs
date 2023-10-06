using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace AirforceAgniVirBackchodLogTracker
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        static string database_Cadet = "AgniveerBookOut.db";
       
        static string folderpath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        public static string databasepath = System.IO.Path.Combine(folderpath, database_Cadet);



       
    }

}
