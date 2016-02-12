using System;
using System.Collections.Generic;
using System.Text;
using PatientCare.Shared;
using PatientCare.Shared.Interfaces;
using PatientCare.Shared.Model;
using PatientCare.Shared.Model.Utils;
using SQLite.Net;
using SQLite.Net.Platform.XamarinIOS;
using UIKit;

namespace PatientCare.iOS
{
    public class LocalDB : SharedLocalDB, ISQLConnection
    {
        public LocalDB()
        {
            // Make the SQL server connection
            db = CreateConnection();
        }

        public string DatabaseFilePath()
        {
            // we need to put in /Library/ on iOS5.1 to meet Apple's iCloud terms
                // (they don't want non-user-generated data in Documents)
                string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal); // Documents folder
                string libraryPath = System.IO.Path.Combine(documentsPath, "../Library/");
                var path = System.IO.Path.Combine(libraryPath, "localcallsdb");

                return path;
        }

        public SQLiteConnection CreateConnection()
        {
            return new SQLiteConnection(new SQLitePlatformIOS(), DatabaseFilePath());
        }

        
    }
}
